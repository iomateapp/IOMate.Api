import { createFileRoute } from '@tanstack/react-router'
import { Outlet, redirect } from '@tanstack/react-router'

import { alpha, Box, Stack } from '@mui/material'
import SideMenu from '../components/SideMenu'
import Header from '../components/Header'
import DashboardLayout from '../components/DashboardLayout'

export const Route = createFileRoute('/_auth')({
  beforeLoad: ({ context, location }) => {
    if (!context.auth.isAuthenticated) {
      throw redirect({
        to: '/login',
        search: {
          redirect: location.href,
        },
      })
    }
  },
  component: AuthLayout,
})

function AuthLayout() {
  return (
    <DashboardLayout>
      <Outlet />
    </DashboardLayout>
  )
}

export function AuthLayoutPrevious() {
  return (
    <Box sx={{ display: 'flex' }}>
      <SideMenu />
      <Box
        component="main"
        sx={(theme) => ({
          flexGrow: 1,
          backgroundColor: theme.vars
            ? `rgba(${theme.vars.palette.background.defaultChannel} / 1)`
            : alpha(theme.palette.background.default, 1),
          overflow: 'auto',
        })}
      >
        <Stack
          spacing={2}
          sx={{
            alignItems: 'center',
            mx: 3,
            pb: 5,
            mt: { xs: 8, md: 0 },
          }}
        >
          <Header />
          {/* <MainGrid /> */}
          <Outlet />
        </Stack>
      </Box>
      {/* <div className="p-2 h-full">
        <h1>Authenticated Route</h1>
        <p>This route's content is only visible to authenticated users.</p>
        <ul className="py-2 flex gap-2">
          <li>
            <Link
              to="/dashboard"
              className="hover:underline data-[status='active']:font-semibold"
            >
              Dashboard
            </Link>
          </li>
          <li>
            <button
              type="button"
              className="hover:underline"
              onClick={handleLogout}
            >
              Logout
            </button>
          </li>
        </ul>
        <hr />
        <Outlet />
      </div> */}
    </Box>
  )
}
