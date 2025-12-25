# Script PowerShell - Correction Automatique des Erreurs
# Exécutez ce script en tant qu'administrateur

Write-Host "?? GestionSalle - Script de Correction Automatique" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# Fonction pour afficher les messages
function Write-Step {
    param($Message, $Color = "Yellow")
    Write-Host "? $Message" -ForegroundColor $Color
}

# Fonction pour afficher les succès
function Write-Success {
    param($Message)
    Write-Host "? $Message" -ForegroundColor Green
}

# Fonction pour afficher les erreurs
function Write-Error-Custom {
    param($Message)
    Write-Host "? $Message" -ForegroundColor Red
}

# Étape 1 : Arrêter l'application
Write-Step "Étape 1: Arrêt de l'application GestionSalle..."
try {
    $processes = Get-Process -Name "GestionSalle" -ErrorAction SilentlyContinue
    if ($processes) {
        $processes | Stop-Process -Force
        Write-Success "Application GestionSalle arrêtée (PID: $($processes.Id -join ', '))"
        Start-Sleep -Seconds 2
    } else {
        Write-Success "Aucune instance de GestionSalle en cours d'exécution"
    }
} catch {
    Write-Error-Custom "Impossible d'arrêter l'application: $_"
}

# Étape 2 : Arrêter tous les processus dotnet liés
Write-Step "Étape 2: Arrêt des processus .NET associés..."
try {
$dotnetProcesses = Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | 
   Where-Object { $_.MainWindowTitle -like "*GestionSalle*" }
    if ($dotnetProcesses) {
        $dotnetProcesses | Stop-Process -Force
 Write-Success "Processus .NET arrêtés"
    }
} catch {
    Write-Error-Custom "Erreur lors de l'arrêt des processus .NET: $_"
}

# Étape 3 : Naviguer vers le dossier du projet
Write-Step "Étape 3: Navigation vers le dossier du projet..."
$projectPath = "C:\Users\redan\source\repos\SlnGestionSalle\GestionSalle"
if (Test-Path $projectPath) {
    Set-Location $projectPath
    Write-Success "Dossier du projet trouvé: $projectPath"
} else {
    Write-Error-Custom "Dossier du projet non trouvé: $projectPath"
    exit 1
}

# Étape 4 : Supprimer les dossiers bin et obj
Write-Step "Étape 4: Nettoyage des dossiers bin et obj..."
try {
    if (Test-Path "bin") {
        Remove-Item -Path "bin" -Recurse -Force
  Write-Success "Dossier bin supprimé"
    }
    if (Test-Path "obj") {
        Remove-Item -Path "obj" -Recurse -Force
        Write-Success "Dossier obj supprimé"
    }
} catch {
    Write-Error-Custom "Erreur lors de la suppression des dossiers: $_"
}

# Étape 5 : dotnet clean
Write-Step "Étape 5: Exécution de 'dotnet clean'..."
try {
 $cleanOutput = dotnet clean 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Success "dotnet clean réussi"
    } else {
    Write-Error-Custom "dotnet clean a échoué: $cleanOutput"
    }
} catch {
    Write-Error-Custom "Erreur lors de dotnet clean: $_"
}

# Étape 6 : dotnet restore
Write-Step "Étape 6: Exécution de 'dotnet restore'..."
try {
    $restoreOutput = dotnet restore 2>&1
    if ($LASTEXITCODE -eq 0) {
  Write-Success "dotnet restore réussi"
    } else {
        Write-Error-Custom "dotnet restore a échoué: $restoreOutput"
    }
} catch {
    Write-Error-Custom "Erreur lors de dotnet restore: $_"
}

# Étape 7 : dotnet build
Write-Step "Étape 7: Exécution de 'dotnet build'..."
try {
    $buildOutput = dotnet build --no-restore 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Success "dotnet build réussi"
    } else {
        Write-Error-Custom "dotnet build a échoué"
     Write-Host $buildOutput
    }
} catch {
    Write-Error-Custom "Erreur lors de dotnet build: $_"
}

# Étape 8 : Vérification des processus restants
Write-Step "Étape 8: Vérification finale..."
$remainingProcesses = Get-Process -Name "GestionSalle" -ErrorAction SilentlyContinue
if ($remainingProcesses) {
    Write-Error-Custom "Des processus GestionSalle sont encore actifs"
} else {
    Write-Success "Aucun processus GestionSalle actif"
}

# Résumé
Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "?? CORRECTION TERMINÉE" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "?? ACTIONS RECOMMANDÉES:" -ForegroundColor Yellow
Write-Host "1. Retournez dans Visual Studio" -ForegroundColor White
Write-Host "2. Appuyez sur F5 pour démarrer l'application" -ForegroundColor White
Write-Host "3. Si problème persiste, redémarrez Visual Studio" -ForegroundColor White
Write-Host ""

# Afficher les informations système
Write-Host "?? INFORMATIONS SYSTÈME:" -ForegroundColor Cyan
Write-Host "Version .NET:" -ForegroundColor White
dotnet --version
Write-Host ""
Write-Host "SDKs .NET installés:" -ForegroundColor White
dotnet --list-sdks | Select-Object -First 5
Write-Host ""

# Pause pour lire les résultats
Write-Host "Appuyez sur une touche pour fermer..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
