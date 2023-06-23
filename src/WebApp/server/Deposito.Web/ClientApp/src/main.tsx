import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import { RouterProvider, createBrowserRouter } from 'react-router-dom';
import DepositListings from './pages/DepositListings.tsx';
import Deposit from './pages/Deposit.tsx';


const router = createBrowserRouter([
	{
		path: "/",
		element: <App />
	},
	{
		path: "/deposit-list/:amount/:currency/:period/:paymentType",
		element: <DepositListings />
	},
	{
		path: "/deposit/:id/:amount",
		element: <Deposit />
	}
]);

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
	<React.StrictMode>
		<RouterProvider router={router} />
	</React.StrictMode>,
)
