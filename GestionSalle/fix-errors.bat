@echo off
echo ========================================
echo  FIX GESTIONSALLE - CORRECTION RAPIDE
echo ========================================
echo.

REM Arrêter l'application
echo [1/7] Arret de l'application...
taskkill /F /IM GestionSalle.exe 2>nul
if %errorlevel% == 0 (
    echo  ? Application arretee
) else (
    echo  ! Aucune instance active
)
timeout /t 2 /nobreak >nul

REM Naviguer vers le dossier du projet
echo.
echo [2/7] Navigation vers le projet...
cd /d "C:\Users\redan\source\repos\SlnGestionSalle\GestionSalle"
if %errorlevel% == 0 (
    echo  ? Dossier trouve
) else (
    echo  ? Erreur: Dossier non trouve
    pause
    exit /b 1
)

REM Supprimer bin
echo.
echo [3/7] Suppression du dossier bin...
if exist bin (
    rmdir /s /q bin
    echo  ? Dossier bin supprime
) else (
    echo  ! Dossier bin inexistant
)

REM Supprimer obj
echo.
echo [4/7] Suppression du dossier obj...
if exist obj (
    rmdir /s /q obj
echo  ? Dossier obj supprime
) else (
    echo  ! Dossier obj inexistant
)

REM dotnet clean
echo.
echo [5/7] Execution de 'dotnet clean'...
dotnet clean >nul 2>&1
if %errorlevel% == 0 (
    echo  ? Clean reussi
) else (
    echo  ? Erreur lors du clean
)

REM dotnet restore
echo.
echo [6/7] Execution de 'dotnet restore'...
dotnet restore >nul 2>&1
if %errorlevel% == 0 (
 echo  ? Restore reussi
) else (
    echo  ? Erreur lors du restore
)

REM dotnet build
echo.
echo [7/7] Execution de 'dotnet build'...
dotnet build --no-restore
if %errorlevel% == 0 (
    echo  ? Build reussi
) else (
    echo  ? Erreur lors du build
    echo.
    echo Verifiez les erreurs ci-dessus
)

REM Résumé
echo.
echo ========================================
echoCORRECTION TERMINEE
echo ========================================
echo.
echo Actions recommandees:
echo 1. Retournez dans Visual Studio
echo 2. Appuyez sur F5 pour demarrer
echo 3. Si probleme, redemarrez Visual Studio
echo.

pause
