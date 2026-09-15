# Azure Deployment Workflows

## App Service Deployment
When deploying ASP.NET Core APIs to Azure App Service:

1. **Deployment Slots**: Deploy to a `staging` slot first. Once warmed up and verified, perform a slot swap to `production`.
2. **Health Checks**: Configure the Azure App Service Health Check path to point to `/health` so the load balancer can verify the instance is alive before routing traffic.
3. **Environment Variables**: Map all required sensitive secrets into the App Service configuration. Do not rely on local `.json` files in Azure.
