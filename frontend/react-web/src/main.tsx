import React from 'react'
import ReactDOM from 'react-dom/client'
import './main.scss'
import {QueryClient, QueryClientProvider} from "@tanstack/react-query";
import App from "./App";


const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: false
    }
  }
});

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <QueryClientProvider client={queryClient}>
      <App/>
    </QueryClientProvider>
  </React.StrictMode>
)
