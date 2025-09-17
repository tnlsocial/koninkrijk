import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import AuthProvider from './services/AuthProvider.jsx'

import './App.css'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap/dist/js/bootstrap.js'

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <AuthProvider>
      <App />
    </AuthProvider>
  </React.StrictMode>,
)
