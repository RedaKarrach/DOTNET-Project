# ?? CORRECTION DES ERREURS - GUIDE COMPLET

## ?? PROBLÈME PRINCIPAL

```
error MSB3027: Could not copy "apphost.exe" to "GestionSalle.exe"
The file is locked by: "GestionSalle (18736)"
```

**CAUSE** : L'application est en cours d'exécution et bloque le processus de build.

---

## ?? SOLUTIONS RAPIDES (Par Ordre de Priorité)

### ? SOLUTION 1 : Arrêt Manuel dans Visual Studio (RECOMMANDÉ)

1. **Appuyez sur `Shift + F5`** (Stop Debugging)
2. **OU** Cliquez sur le bouton rouge ? dans la barre d'outils
3. Attendez 5 secondes
4. Faites `F5` pour redémarrer

**TEMPS** : 10 secondes
**DIFFICULTÉ** : ? Facile

---

### ?? SOLUTION 2 : Script Automatique Batch (ULTRA RAPIDE)

1. Double-cliquez sur le fichier **`fix-errors.bat`**
2. Le script fait tout automatiquement :
   - Arrête l'application
   - Nettoie les dossiers
   - Restore et build le projet
3. Retournez dans Visual Studio
4. Appuyez sur `F5`

**TEMPS** : 30 secondes
**DIFFICULTÉ** : ? Facile

---

### ?? SOLUTION 3 : Script PowerShell (COMPLET)

1. Cliquez droit sur **`fix-errors.ps1`**
2. Sélectionnez "Exécuter avec PowerShell"
3. Si erreur "Scripts désactivés", exécutez d'abord :
   ```powershell
   Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
   ```
4. Relancez le script

**TEMPS** : 1 minute
**DIFFICULTÉ** : ?? Moyen

---

### ??? SOLUTION 4 : Task Manager

1. Ouvrez le Gestionnaire des tâches : `Ctrl + Shift + Esc`
2. Onglet "Processus"
3. Cherchez "GestionSalle.exe"
4. Cliquez droit ? **Fin de tâche**
5. Retournez dans Visual Studio ? `F5`

**TEMPS** : 20 secondes
**DIFFICULTÉ** : ? Facile

---

### ?? SOLUTION 5 : Commandes Manuelles

Ouvrez PowerShell dans le dossier du projet :

```powershell
# 1. Arrêter l'application
taskkill /F /IM GestionSalle.exe

# 2. Nettoyer
dotnet clean

# 3. Supprimer les dossiers
Remove-Item -Recurse -Force bin, obj

# 4. Restore
dotnet restore

# 5. Build
dotnet build
```

**TEMPS** : 2 minutes
**DIFFICULTÉ** : ?? Moyen

---

## ?? AUTRES ERREURS DÉTECTÉES

### ?? Vulnérabilité Microsoft.Build

**Warning** :
```
warning NU1903: Package 'Microsoft.Build' 17.11.4 has a known 
high severity vulnerability
```

**SOLUTION** :

#### Option A : Via NuGet Package Manager (GUI)
1. Cliquez droit sur le projet ? **Manage NuGet Packages**
2. Onglet "Updates"
3. Cherchez "Microsoft.Build"
4. Cliquez sur "Update"

#### Option B : Via fichier .csproj
Ouvrez `GestionSalle.csproj` et modifiez :

```xml
<!-- AVANT -->
<PackageReference Include="Microsoft.Build" Version="17.11.4" />

<!-- APRÈS -->
<PackageReference Include="Microsoft.Build" Version="17.12.6" />
```

Puis :
```bash
dotnet restore
dotnet build
```

---

### ? .NET 10 Non Supporté dans VS 2022 17.14

**Warning** :
```
warning NETSDK1233: Targeting .NET 10.0 or higher in 
Visual Studio 2022 17.14 is not supported
```

**CONTEXTE** :
- .NET 10 est en preview
- Visual Studio 2022 17.14 ne le supporte pas officiellement
- **Ce n'est qu'un WARNING, pas une erreur critique**

**SOLUTIONS** :

#### Option A : Mettre à jour Visual Studio (RECOMMANDÉ)
1. Ouvrez **Visual Studio Installer**
2. Cliquez sur "Modifier" pour VS 2022
3. Onglet "Composants individuels"
4. Cherchez ".NET 10 Preview SDK"
5. Cochez et installez
6. Redémarrez Visual Studio

#### Option B : Downgrade vers .NET 9 (STABLE)

Dans `GestionSalle.csproj` :

```xml
<!-- AVANT -->
<TargetFramework>net10.0</TargetFramework>

<!-- APRÈS -->
<TargetFramework>net9.0</TargetFramework>
```

Puis :
```bash
dotnet restore
dotnet clean
dotnet build
```

#### Option C : Ignorer le Warning
Si vous voulez continuer avec .NET 10 malgré le warning :

Dans `GestionSalle.csproj`, ajoutez :

```xml
<PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <NoWarn>NETSDK1233</NoWarn>
</PropertyGroup>
```

---

## ?? CHECKLIST COMPLÈTE

### Avant de Coder
- [ ] Vérifiez qu'aucune instance de GestionSalle.exe n'est active
- [ ] Vérifiez que Visual Studio est à jour
- [ ] Vérifiez que les packages NuGet sont à jour

### Si Erreur de Build
- [ ] Arrêtez l'application (`Shift + F5`)
- [ ] Clean Solution (`Build` ? `Clean Solution`)
- [ ] Rebuild Solution (`Build` ? `Rebuild Solution`)
- [ ] Si problème persiste, exécutez `fix-errors.bat`

### Si Erreur Persiste
- [ ] Fermez Visual Studio
- [ ] Exécutez `fix-errors.bat` OU `fix-errors.ps1`
- [ ] Supprimez manuellement les dossiers `bin` et `obj`
- [ ] Redémarrez Visual Studio
- [ ] Ouvrez le projet et faites `F5`

### En Dernier Recours
- [ ] Redémarrez votre PC
- [ ] Vérifiez les mises à jour Windows
- [ ] Vérifiez l'antivirus (peut bloquer les fichiers)

---

## ?? BONNES PRATIQUES

### Pour Éviter ce Problème à l'Avenir

1. **Toujours arrêter avant de rebuild**
   ```
   Shift + F5 ? Build ? Rebuild
   ```

2. **Activez Hot Reload**
   ```
   Tools ? Options ? Debugging ? .NET Hot Reload
   ? Enable Hot Reload when debugging
   ```

3. **Utilisez les builds incrémentiels**
   - Faites `Build` au lieu de `Rebuild` quand possible
   - `Rebuild` supprime et reconstruit tout (plus lent)

4. **Nettoyez régulièrement**
   ```
   Une fois par semaine:
   Build ? Clean Solution
   ```

---

## ?? AIDE SUPPLÉMENTAIRE

### Si Rien Ne Fonctionne

1. **Vérifiez les logs détaillés**
   ```
 Tools ? Options ? Projects and Solutions ? Build and Run
   MSBuild project build output verbosity: Detailed
   ```

2. **Exécutez le Repair de Visual Studio**
   ```
   Visual Studio Installer ? More ? Repair
   ```

3. **Réinstallez .NET SDK**
 ```
   Téléchargez depuis: https://dotnet.microsoft.com/download
   ```

4. **Contactez le support**
   - GitHub Issues : [Créer un ticket]
- Stack Overflow : [Poster une question]

---

## ?? RÉSUMÉ DES FICHIERS

| Fichier | Description | Usage |
|---------|-------------|-------|
| `fix-errors.bat` | Script Windows Batch | Double-cliquez |
| `fix-errors.ps1` | Script PowerShell | Clic droit ? Run with PowerShell |
| `ERROR_FIX_GUIDE.md` | Guide détaillé | Documentation |

---

## ? VÉRIFICATION FINALE

Après correction, vérifiez :

```bash
# 1. Vérifier qu'aucun processus n'est actif
tasklist | findstr GestionSalle

# 2. Vérifier que le build fonctionne
dotnet build

# 3. Vérifier les warnings
dotnet build --verbosity detailed

# 4. Lancer l'application
dotnet run
```

---

## ?? COMMANDE UNIQUE MAGIQUE

Si vous voulez tout faire en une seule commande :

```powershell
taskkill /F /IM GestionSalle.exe 2>$null; `
Remove-Item -Recurse -Force bin,obj -ErrorAction SilentlyContinue; `
dotnet clean; dotnet restore; dotnet build; `
Write-Host "? Prêt! Appuyez sur F5 dans Visual Studio" -ForegroundColor Green
```

Copiez-collez dans PowerShell depuis le dossier du projet !

---

**Dernière mise à jour** : 2025
**Testé sur** : Windows 11, Visual Studio 2022, .NET 10 Preview
**Status** : ? VÉRIFIÉ ET FONCTIONNEL
