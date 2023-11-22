import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import { CrudProvider } from './Context/CrudContext.jsx'


ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <CrudProvider>
      <App />
    </CrudProvider>
  </React.StrictMode>,
)
