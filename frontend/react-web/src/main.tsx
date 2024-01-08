import React from 'react'
import ReactDOM from 'react-dom/client'
import './main.scss'
import {QueryClient, QueryClientProvider} from "@tanstack/react-query";
import App from "./App";
import {LoaderProvider} from "./contexts/LoaderContext.tsx";
import {Loader} from "./contexts/Loader.tsx";


const queryClient = new QueryClient();

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <QueryClientProvider client={queryClient}>
      <LoaderProvider>
        <Loader/>
        <App/>
      </LoaderProvider>
    </QueryClientProvider>
  </React.StrictMode>
)
