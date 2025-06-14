$gatewayRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition
$root = Split-Path -Parent $gatewayRoot

$subgraphProjects = @(
    "scheduler/src/graph-exporter",
    "identity/src/graph-exporter"
)

foreach ($subgraphProject in $subgraphProjects) {
    $fullPath = Join-Path $root $subgraphProject

    Write-Host "Exporting schema in $fullPath..."
    Push-Location $fullPath

    dotnet run -- schema export --output schema.graphql
    fusion subgraph pack

    Pop-Location
}

$supergraphPath = Join-Path $gatewayRoot "src/root/Gateway.fgp"

foreach ($subgraphProject in $subgraphProjects) {
    $fullPath = Join-Path $root $subgraphProject

    Write-Host "Composing schema from $fullPath..."
    fusion compose -p $supergraphPath -s $fullPath 
}

Write-Host "GraphQL schema composed completed for all projects."
