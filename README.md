# CITADEL_NEXUS_PUBLIC

Public Frontier showroom built with **.NET 9 Blazor WebAssembly** and **MudBlazor**.

## Included modules
- Sovereign high-contrast (Gold/Black/White) enterprise UI
- Landing page describing Citadel architecture and T.I.A. distributed intelligence role
- Restricted public chat module with sanity filtering and Public Oracle routing
- Multimedia portfolio showcase page for future Adobe/VTS assets
- Static deployment workflow: `.github/workflows/deploy_frontier.yml` (Vercel)

## Secure configuration
Public Oracle settings are provided through public config files (`wwwroot/appsettings*.json`):

```json
"PublicOracle": {
  "Endpoint": "https://public-oracle.your-hub.example/api/chat",
  "PublicApiKey": "REPLACE_WITH_PUBLIC_API_KEY"
}
```

No private keys or internal hub URLs are hardcoded in the app.

## Preview
![Frontier Landing](https://github.com/user-attachments/assets/6ab0c638-ff10-4885-9ced-27a94c0cd90f)
