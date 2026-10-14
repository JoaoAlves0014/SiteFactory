# Script para enviar o README do Perfil para https://github.com/JoaoAlves0014/JoaoAlves0014

$SiteDir = $PSScriptRoot
$ProfileDir = Join-Path $SiteDir "profile-repo"

Copy-Item (Join-Path $SiteDir "PROFILE_README.md") (Join-Path $ProfileDir "README.md") -Force

Set-Location $ProfileDir
git add README.md
git commit -m "feat: atualizar README do perfil JoaoAlves0014" 2>$null

Write-Host "=========================================================" -ForegroundColor Cyan
Write-Host "  Publicando Perfil do GitHub: JoaoAlves0014" -ForegroundColor White
Write-Host "=========================================================" -ForegroundColor Cyan
Write-Host "Enviando para o GitHub (JoaoAlves0014/JoaoAlves0014)..." -ForegroundColor Yellow

git push -u origin main

if ($LASTEXITCODE -eq 0) {
    Write-Host "`n✔ Perfil publicado com sucesso no GitHub!" -ForegroundColor Green
    Write-Host "Confira agora em: https://github.com/JoaoAlves0014" -ForegroundColor Cyan
} else {
    Write-Host "`n⚠ O repositório precisa ser criado no GitHub primeiro:" -ForegroundColor Yellow
    Write-Host "1. Acesse: https://github.com/new" -ForegroundColor White
    Write-Host "2. Nome: JoaoAlves0014" -ForegroundColor Green
    Write-Host "3. Marque 'Public'" -ForegroundColor White
    Write-Host "4. Clique em 'Create repository' e execute este script novamente." -ForegroundColor White
}
