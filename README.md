# AquaFarm Project

## Overview
Manage aqua farms and workers via a React frontend and ASP.NET Core backend.

## Features

- List fish farms with pagination.
- Register new fish farms with image upload.
- View and add workers per fish farm.
- Worker registration includes image (optional) and role selection.

## Backend
- ASP.NET Core Web API, EF Core, AutoMapper, xUnit  
- Runs on `http://localhost:5124`  
- Images stored in `wwwroot/images/` (add to `.gitignore`)  
- **CORS:** Already enabled for frontend URL. If the URL changes, ensure CORS is updated in `Program.cs`.

## Database
- Ensure the database connection in appsettings.Development.json.
```bash
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=AquaFarmDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

## Frontend
- React + Vite, developed on Node.js v22.18.0  
- MUI, React Query, React Hook Form, Axios  
- Runs on `http://localhost:5173`  
- Backend URL configurable in `src/config.js`.

## Setup & Run

### Backend
1. Backend project location: `root/backend`
2. Set `AquaFarm.API` as the start-up project.
3. Ensure `root/AquaFarm.API/wwwroot/images/` exists for image uploads.
4. Build and run the API.

### Frontend
1. Navigate to client app folder: `root/client`
```bash
npm install
npm run dev
