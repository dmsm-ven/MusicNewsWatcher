<#
.SYNOPSIS
    Пересобирает образ API и запускает docker compose одной командой.

.DESCRIPTION
    Кладите файл в корень репозитория (рядом с docker-compose.yml) и запускайте:
        .\start-docker.ps1               # пересобрать образ API и (пере)запустить compose
        .\start-docker.ps1 -NoBuild      # только запустить compose, без сборки образа
        .\start-docker.ps1 -NoCache      # собрать образ с нуля, без кэша слоёв
        .\start-docker.ps1 -Logs         # после запуска показывать логи API (Ctrl+C - выйти из логов)

    Образ собирается под тем же именем, которое указано в docker-compose.yml (painven/mnwatcher_api:latest),
    поэтому сам compose-файл менять не нужно.
#>
[CmdletBinding()]
param(
    [switch]$NoBuild,
    [switch]$NoCache,
    [switch]$Logs
)

$ErrorActionPreference = 'Stop'
Set-Location -Path $PSScriptRoot

$ImageName  = 'painven/mnwatcher_api:latest'
$Dockerfile = 'MusicNewsWatcher.API/Dockerfile'
$ApiService = 'api'

function Write-Step([string]$Text) { Write-Host "==> $Text" -ForegroundColor Cyan }

function Assert-Success([string]$What) {
    if ($LASTEXITCODE -ne 0) { throw "$What завершилась с ошибкой (код $LASTEXITCODE)" }
}

try {
    # --- Проверки окружения -------------------------------------------------
    if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
        throw 'Docker не найден в PATH. Установите/запустите Docker Desktop.'
    }

    docker info *> $null
    if ($LASTEXITCODE -ne 0) {
        throw 'Docker не отвечает. Запустите Docker Desktop и повторите.'
    }

    foreach ($required in 'docker-compose.yml', $Dockerfile, 'api.env', 'db.env') {
        if (-not (Test-Path -LiteralPath $required)) {
            throw "Не найден файл '$required'. Запускайте скрипт из корня проекта (рядом с docker-compose.yml)."
        }
    }

    # Чтобы скрипт случайно не попал в коммит: добавляем его в локальный .git/info/exclude (в репозиторий это не уходит)
    $exclude = Join-Path $PSScriptRoot '.git/info/exclude'
    if (Test-Path -LiteralPath $exclude) {
        $self = $MyInvocation.MyCommand.Name
        if (-not (Select-String -LiteralPath $exclude -SimpleMatch -Pattern $self -Quiet)) {
            Add-Content -LiteralPath $exclude -Value $self
        }
    }

    # --- Сборка образа API --------------------------------------------------
    if (-not $NoBuild) {
        Write-Step "Сборка образа $ImageName"

        $buildArgs = @('build', '-f', $Dockerfile, '-t', $ImageName)
        if ($NoCache) { $buildArgs += '--no-cache' }
        $buildArgs += '.'   # контекст сборки - корень репозитория (Dockerfile копирует файлы из нескольких проектов)

        & docker @buildArgs
        Assert-Success 'Сборка образа'

        # Прежний образ с тем же тегом становится "висячим" - убираем, чтобы не копились
        docker image prune -f *> $null
    }

    # --- Запуск compose -----------------------------------------------------
    Write-Step 'Запуск docker compose'

    if ($NoBuild) {
        docker compose up -d --remove-orphans
    }
    else {
        # Тег образа не изменился, поэтому явно пересоздаём контейнер API, чтобы он взял свежий образ
        docker compose up -d --remove-orphans --force-recreate $ApiService
    }
    Assert-Success 'docker compose up'

    Write-Step 'Состояние контейнеров'
    docker compose ps

    Write-Host ''
    Write-Host 'Готово. API: http://localhost:8050  (Swagger: http://localhost:8050/swagger)' -ForegroundColor Green

    if ($Logs) {
        Write-Step 'Логи API (Ctrl+C - выход)'
        docker compose logs -f --tail 100 $ApiService
    }
    else {
        Write-Host "Логи API:  docker compose logs -f $ApiService" -ForegroundColor DarkGray
    }
}
catch {
    Write-Host ''
    Write-Host "Ошибка: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}
