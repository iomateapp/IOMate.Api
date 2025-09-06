import * as React from 'react'

export interface AuthContext {
  isAuthenticated: boolean
  setToken: (token: string, refreshToken: string) => void
  logout: () => Promise<void>
  token: string | null
}

const AuthContext = React.createContext<AuthContext | null>(null)

const key = 'auth.token'

function getStoredToken() {
  return localStorage.getItem(key)
}

function setStoredToken(token: string | null) {
  if (token) {
    localStorage.setItem(key, token)
  } else {
    localStorage.removeItem(key)
  }
}


export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [token, localSetToken] = React.useState<string | null>(getStoredToken())
  const isAuthenticated = !!token

  const logout = React.useCallback(async () => {
    setStoredToken(null)
    localSetToken(null)
  }, [])

  const setToken = React.useCallback((token: string, refreshToken: string) => {
    console.log('Setting token:', { token, refreshToken })
    setStoredToken(token)
    localSetToken(token)
  }, [])

  React.useEffect(() => {
    localSetToken(getStoredToken())
  }, [])

  return (
    <AuthContext.Provider value={{ isAuthenticated, token, setToken, logout }}>
      {children}
    </AuthContext.Provider>
  )
}

// eslint-disable-next-line react-refresh/only-export-components
export function useAuth() {
  const context = React.useContext(AuthContext)
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider')
  }
  return context
}
