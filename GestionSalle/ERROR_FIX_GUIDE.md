# ?? GUIDE DE CORRECTION DES ERREURS

## ? ERREURS CRITIQUES DÉTECTÉES

### 1. **APPLICATION EN COURS D'EXÉCUTION**

**Erreur** :
```
error MSB3027: Could not copy "apphost.exe" to "GestionSalle.exe". 
The file is locked by: "GestionSalle (18736)"
```

**SOLUTION IMMÉDIATE** :

#### Option A : Arrêter l'application dans Visual Studio
1. Appuyez sur `Shift + F5` (Stop Debugging)
2. OU cliquez sur le bouton rouge ? (Stop) dans la barre d'outils
3. Attendez 5 secondes
4. Relancez avec `F5`

#### Option B : Fermer via le Task Manager
1. Ouvrez le Gestionnaire des tâches (`Ctrl + Shift + Esc`)
2. Cherchez "GestionSalle.exe" dans l'onglet Processus
3. Cliquez droit ? Fin de tâche
4. Retournez dans Visual Studio et faites `F5`

#### Option C : Via PowerShell (Méthode rapide)
```powershell
# Dans PowerShell (en tant qu'administrateur)
taskkill /F /IM GestionSalle.exe
```

### 2. **VULNÉRABILITÉ DE SÉCURITÉ**

**Warning** :
```
warning NU1903: Package 'Microsoft.Build' 17.11.4 has a known 
high severity vulnerability
```

**SOLUTION** :

Mettez à jour le package dans le fichier `.csproj` :

```xml
<PackageReference Include="Microsoft.Build" Version="17.12.6" />
```

OU via NuGet Package Manager :
```
Tools > NuGet Package Manager > Manage NuGet Packages for Solution
Cherchez "Microsoft.Build" et mettez à jour vers la dernière version
```

### 3. **.NET 10 NON SUPPORTÉ**

**Warning** :
```
warning NETSDK1233: Targeting .NET 10.0 or higher in 
Visual Studio 2022 17.14 is not supported.
```

**CONTEXTE** :
- .NET 10 est en preview
- Visual Studio 2022 17.14 ne le supporte pas officiellement
- Vous pouvez continuer à développer MAIS avec des warnings

**SOLUTIONS** :

#### Solution 1 : Mettre à jour Visual Studio (RECOMMANDÉ)
1. Ouvrez Visual Studio Installer
2. Cliquez sur "Modifier"
3. Allez dans "Composants individuels"
4. Cherchez ".NET 10 Preview SDK"
5. Installez et redémarrez

#### Solution 2 : Downgrade vers .NET 9 (STABLE)
```xml
<!-- Dans GestionSalle.csproj -->
<TargetFramework>net9.0</TargetFramework>
```

Puis exécutez :
```bash
dotnet restore
dotnet clean
dotnet build
```

## ?? CHECKLIST DE CORRECTION

### Étape 1 : Arrêter l'Application
- [ ] Appuyez sur `Shift + F5`
- [ ] Vérifiez dans le Task Manager que GestionSalle.exe est fermé
- [ ] Attendez 5 secondes

### Étape 2 : Clean & Rebuild
- [ ] Dans Visual Studio : `Build` ? `Clean Solution`
- [ ] Puis : `Build` ? `Rebuild Solution`
- [ ] Vérifiez qu'il n'y a pas d'erreurs

### Étape 3 : Corriger la Vulnérabilité (Optionnel mais recommandé)
- [ ] Ouvrez `GestionSalle.csproj`
- [ ] Trouvez la ligne `Microsoft.Build`
- [ ] Changez la version vers `17.12.6` ou supérieure
- [ ] Sauvegardez et faites `dotnet restore`

### Étape 4 : Gérer le Warning .NET 10 (Optionnel)
- [ ] Option A : Mettre à jour Visual Studio vers 17.15+
- [ ] Option B : Downgrade vers .NET 9.0
- [ ] Option C : Ignorer le warning (pas critique)

### Étape 5 : Relancer l'Application
- [ ] Appuyez sur `F5`
- [ ] Vérifiez que tout fonctionne correctement

## ?? COMMANDES RAPIDES

### Nettoyage Complet
```bash
# PowerShell dans le dossier du projet
dotnet clean
dotnet restore
dotnet build
```

### Tuer Tous les Processus .NET
```powershell
# PowerShell (Admin)
Get-Process -Name *GestionSalle* | Stop-Process -Force
```

### Vérifier la Version de .NET
```bash
dotnet --version
dotnet --list-sdks
```

## ?? DIAGNOSTIC

### Si le problème persiste :

1. **Redémarrez Visual Studio**
   ```
   Fichier ? Quitter
   Attendez 10 secondes
   Relancez Visual Studio
   ```

2. **Supprimez les dossiers temporaires**
   ```bash
   # Dans le dossier du projet
   Remove-Item -Recurse -Force bin
   Remove-Item -Recurse -Force obj
   dotnet restore
   dotnet build
   ```

3. **Redémarrez votre PC**
   - Parfois des processus zombies persistent
   - Un redémarrage complet résout le problème

## ?? RÉSUMÉ DES ERREURS

| Erreur | Sévérité | Status | Action |
|--------|----------|--------|--------|
| Application verrouillée | ?? CRITIQUE | À corriger | Arrêter l'app |
| Vulnérabilité Microsoft.Build | ?? HAUTE | Recommandé | Mettre à jour |
| .NET 10 non supporté | ?? INFO | Optionnel | Ignorer ou downgrade |

## ? VÉRIFICATION FINALE

Après les corrections, vérifiez que :

- [ ] Le build réussit sans erreurs
- [ ] L'application démarre correctement
- [ ] Pas de processus GestionSalle.exe zombie
- [ ] Tous les CSS/JS sont chargés
- [ ] La base de données est accessible

## ?? CONSEIL PRO

Pour éviter ce problème à l'avenir :

1. **Toujours arrêter l'application avant de rebuild**
 - Utilisez `Shift + F5` systématiquement

2. **Activez Hot Reload**
   ```
   Tools ? Options ? Debugging ? .NET Hot Reload
   Cochez "Enable Hot Reload when debugging"
   ```

3. **Utilisez le mode Release pour les tests finaux**
   ```
   Debug ? GestionSalle Debug Properties
   Configuration : Release
   ```

## ?? ACTIONS IMMÉDIATES

**FAITES CECI MAINTENANT** :

1. ?? Arrêtez l'application (`Shift + F5`)
2. ?? Clean Solution (`Build` ? `Clean Solution`)
3. ?? Rebuild Solution (`Build` ? `Rebuild Solution`)
4. ?? Redémarrez (`F5`)

---

**Status** : ?? ERREURS ACTIVES
**Priorité** : CRITIQUE - Arrêter l'application en cours
**ETA** : 2 minutes pour corriger
