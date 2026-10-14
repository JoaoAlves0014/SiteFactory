# Script para enviar o portfólio para https://github.com/JoaoAlves0014/joaoalves0014.github.io

Write-Host "=========================================================" -ForegroundColor Cyan
Write-Host "  Enviando Portfólio para GitHub Pages: JoaoAlves0014" -ForegroundColor White
Write-Host "=========================================================" -ForegroundColor Cyan

$SiteDir = Join-Path $PSScriptRoot "site"

Set-Location $SiteDir
Write-Host "Executando: git push -u origin main..." -ForegroundColor Yellow
git push -u origin main

if ($LASTEXITCODE -eq 0) {
    Write-Host "`n✔ Portfólio enviado com sucesso!" -ForegroundColor Green
    Write-Host "Em instantes o site estará no ar em: https://joaoalves0014.github.io" -ForegroundColor Cyan
} else {
    Write-Host "`n⚠ Se o repositório ainda não existir no GitHub:" -ForegroundColor Yellow
    Write-Host "1. Crie o repositório 'joaoalves0014.github.io' em https://github.com/new" -ForegroundColor White
    Write-Host "2. Execute este script novamente." -ForegroundColor White
}
