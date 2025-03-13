# E-commerce App Dotnet

## Tech Stack

- **C#**
- **.NET 8.0**
- **MongoDB**
- **React**
- **Typescript**
- **Tailwind**
- **Vite**
- **MinIO**
- **Firebase Authentication**

## Link to Website

https://www.e-commerce-app-dotnet.pawelsobon.pl

## Example Image

![page](https://github.com/xNTFx/Shopping-page/assets/135262384/a14fba0f-223c-4cd1-9678-899687d092a7)

## Installation

### Prerequisites
- .NET SDK 8.0
- Node.js & npm
- MongoDB instance
- MinIO instance

### Steps
1. Clone the repository:
   ```sh
   git clone https://github.com/xNTFx/e-commerce-app-dotnet.git
   ```
2. Navigate to the project directory:
   ```sh
   cd e-commerce-app-dotnet
   ```
3. Install backend dependencies:
   ```sh
   dotnet restore
   ```
4. Configure environment variables (see the section below).
5. Run the backend:
   ```sh
   dotnet run
   ```
6. Navigate to the frontend directory:
   ```sh
   cd client
   ```
7. Install frontend dependencies:
   ```sh
   npm install
   ```
8. Start the frontend:
   ```sh
   npm run dev
   ```

## Environment Variables

Create a file named `appsettings.json` and configure it with the following values:

```json
{
  "Server": {
    "Port": 8080  // port where the application server runs
  },
  "Application": {
    "Name": "ECommerceApp"  // name of the application
  },
  "MongoDB": {
    "Uri": "",  // connection string for MongoDB database
    "DatabaseName": ""  // name of the MongoDB database
  },
  "Stripe": {
    "PublishableKey": "",  // public API key for Stripe payments
    "SecretKey": ""  // private API key for Stripe payments
  },
  "Google": {
    "ClientEmail": "",  // Google API client email
    "PrivateKey": "",  // Google API private key
    "ProjectId": ""  // Google project identifier
  },
  "Environment": {
    "ActiveProfile": "production"  // active environment profile
  }
}
```

### Running the Application

- **Backend** runs on `http://localhost:8080` (unless configured otherwise in `appsettings.json`).
- **Frontend** runs on `http://localhost:5173` (default Vite port).

