import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { RouterProvider, createRouter } from '@tanstack/react-router'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { CssBaseline } from '@mui/material'
import { routeTree } from './routeTree.gen'
import { AuthProvider, useAuth } from './context/auth'
import { Toaster } from 'react-hot-toast';

import '@fontsource/roboto/300.css'
import '@fontsource/roboto/400.css'
import '@fontsource/roboto/500.css'
import '@fontsource/roboto/700.css'
import AppTheme from './theme/AppTheme'

const queryClient = new QueryClient()

// Create a new router instance
const router = createRouter({ 
  routeTree,
  defaultPreload: 'intent',
  defaultPreloadStaleTime: 0,
  scrollRestoration: true,
  context: {
    auth: undefined!,
    queryClient,
  },
});

// Register the router instance for type safety
declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router
  }
}

// eslint-disable-next-line react-refresh/only-export-components
function App() {
  const auth = useAuth()
  return (
    <QueryClientProvider client={queryClient}>
        <RouterProvider router={router} context={{ auth }} />
    </QueryClientProvider>
  )
}

const rootElement = document.getElementById('root')!
if (!rootElement.innerHTML) {
  const root = createRoot(rootElement)
  root.render(
    <StrictMode>
      <AuthProvider>
        <AppTheme>
          <CssBaseline />
          <App />
          <Toaster />
        </AppTheme>
      </AuthProvider>
    </StrictMode>,
  )
}
