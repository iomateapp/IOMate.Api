import { createFileRoute } from '@tanstack/react-router'
import { 
  Box, 
  Button, 
  CssBaseline, 
  FormLabel, 
  FormControl, 
  TextField, 
  Typography, 
  Stack, 
  Card as MuiCard, 
} from '@mui/material';
import { styled } from '@mui/material/styles';
import { redirect, useRouter, useRouterState } from '@tanstack/react-router'
import toast from 'react-hot-toast';
import { useForm } from "react-hook-form";
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod'
import { useAuth } from '../context/auth'
import { ApiError } from '../services/errors/types';
import { useMutation } from '@tanstack/react-query';
import { loginRequest, type LoginResponse } from '../services/auth';

const fallback = '/dashboard' as const

export const Route = createFileRoute('/login')({
  validateSearch: z.object({
    redirect: z.string().optional().catch(''),
  }),
  beforeLoad: ({ context, search }) => {
    if (context.auth.isAuthenticated) {
      throw redirect({ to: search.redirect || fallback })
    }
  },
  component: LoginComponent,
})

const Card = styled(MuiCard)(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  alignSelf: 'center',
  width: '100%',
  padding: theme.spacing(4),
  gap: theme.spacing(2),
  margin: 'auto',
  [theme.breakpoints.up('sm')]: {
    maxWidth: '450px',
  },
  boxShadow:
    'hsla(220, 30%, 5%, 0.05) 0px 5px 15px 0px, hsla(220, 25%, 10%, 0.05) 0px 15px 35px -5px',
  ...theme.applyStyles('dark', {
    boxShadow:
      'hsla(220, 30%, 5%, 0.5) 0px 5px 15px 0px, hsla(220, 25%, 10%, 0.08) 0px 15px 35px -5px',
  }),
}));

const SignInContainer = styled(Stack)(({ theme }) => ({
  height: 'calc((1 - var(--template-frame-height, 0)) * 100dvh)',
  minHeight: '100%',
  padding: theme.spacing(2),
  [theme.breakpoints.up('sm')]: {
    padding: theme.spacing(4),
  },
  '&::before': {
    content: '""',
    display: 'block',
    position: 'absolute',
    zIndex: -1,
    inset: 0,
    backgroundImage:
      'radial-gradient(ellipse at 50% 50%, hsl(210, 100%, 97%), hsl(0, 0%, 100%))',
    backgroundRepeat: 'no-repeat',
    ...theme.applyStyles('dark', {
      backgroundImage:
        'radial-gradient(at 50% 50%, hsla(210, 100%, 16%, 0.5), hsl(220, 30%, 5%))',
    }),
  },
}));


function LoginComponent() {
  const auth = useAuth()
  const router = useRouter()
  const isLoading = useRouterState({ select: (s) => s.isLoading })
  const navigate = Route.useNavigate()

  const search = Route.useSearch()

  const loginMutation = useMutation<LoginResponse, ApiError, { email: string, password: string}>({
    mutationFn: ({email, password}) => loginRequest(email, password),
    onSuccess: (data) => {
      auth.setToken(data.token, data.refreshToken)
    },
    onError: (error) => {
      if (error instanceof ApiError) {
        console.error('Error logging in: ', error.response)
        toast.error(error.response.Message)
      }
    }
  })

  const schema = z.object({
    email: z.string()
      .trim()
      .toLowerCase()
      .min(1, 'Email is required')
      .email('Invalid email'),
    password: z.string()
      .min(6, 'Password must be at least 6 characters'),
  })

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: zodResolver(schema),
    mode: 'onBlur',
  })

  const onSubmit = handleSubmit(async (values) => {
    console.log('Submitting', values)
    await loginMutation.mutateAsync(values)
    await router.invalidate()
    await navigate({ to: search.redirect || fallback })
  })

  const isLoggingIn = isLoading || loginMutation.isPending;

  return (
    <>
      <CssBaseline enableColorScheme />
      <SignInContainer direction="column" justifyContent="space-between">
        <Card variant="outlined">
          <Typography
            component="h1"
            variant="h4"
            sx={{ width: '100%', fontSize: 'clamp(2rem, 10vw, 2.15rem)' }}
          >
            Sign in
          </Typography>
          {search.redirect ? (
            <p className="text-red-500">You need to login to access this page.</p>
          ) : (
            <p>Login to see all the cool content in here.</p>
          )}
          <Box
            component="form"
            onSubmit={onSubmit}
            noValidate
            sx={{
              display: 'flex',
              flexDirection: 'column',
              width: '100%',
              gap: 2,
            }}
          >
            <FormControl>
              <FormLabel htmlFor="email">Email</FormLabel>
              <TextField
                id="email"
                type="email"
                placeholder="your@email.com"
                autoComplete="email"
                autoFocus
                required
                fullWidth
                variant="outlined"
                error={!!errors.email}
                helperText={errors.email ? errors.email.message : ''}
                {...register('email')} 
              />
            </FormControl>
            <FormControl>
              <FormLabel htmlFor="password">Password</FormLabel>
              <TextField
                placeholder="••••••"
                type="password"
                id="password"
                autoComplete="current-password"
                required
                fullWidth
                variant="outlined"
                error={!!errors.password}
                helperText={errors.password ? errors.password.message : ''}
                {...register('password')} 
              />
            </FormControl>
            <Button
              type="submit"
              fullWidth
              variant="contained"
            >
              {isLoggingIn ? 'Loading...' : 'Login'}
            </Button>
          </Box>
        </Card>
      </SignInContainer>
    </>
  )
}