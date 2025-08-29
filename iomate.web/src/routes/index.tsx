import { createFileRoute, redirect } from '@tanstack/react-router'

// This is the main route, we will never keep the user in the main route, we will always redirect them
export const Route = createFileRoute('/')({
  beforeLoad: ({ context, location }) => {
    if (!context.auth.isAuthenticated) {
      throw redirect({
        to: '/login',
        search: {
          redirect: location.href,
        },
      })
    } 

    throw redirect({ to: '/dashboard' })
  },
})
