@echo off
if "%~1" == "" goto gettarget
set ASSETTARGET=%1
goto createlinks

:gettarget
set "ASSETTARGET="
set /p "ASSETTARGET=Project Assets path (empty to quit): "
if [%ASSETTARGET%]==[] goto abort
if exist "%ASSETTARGET%\" goto createlinks
@echo Project asset folder does not exist
goto gettarget

:createlinks
@echo Clearing previous links...
rmdir "%ASSETTARGET%\Global-Scripts\Common" /S /Q >nul 2>&1
del "%ASSETTARGET%\Global-Scripts\Common.meta" /S >nul 2>&1
rmdir "%ASSETTARGET%\Global-Scripts\Editor" /S /Q >nul 2>&1
del "%ASSETTARGET%\Global-Scripts\Editor.meta" /S >nul 2>&1
rmdir "%ASSETTARGET%\ScriptTemplates" /S /Q >nul 2>&1
del "%ASSETTARGET%\ScriptTemplates.meta" /S >nul 2>&1

@echo Mapping Global Common Script...
mkdir "%ASSETTARGET%\Global-Scripts"
mklink /J "%ASSETTARGET%\Global-Scripts\Common" "E:\Dev\DAG\Global-Resources\Global-Scripts\Common"

@echo Mapping Global Editor Scripts...
mklink /J "%ASSETTARGET%\Global-Scripts\Editor" "E:\Dev\DAG\Global-Resources\Global-Scripts\Editor"

@echo Mapping Script Templates...
mklink /J "%ASSETTARGET%\ScriptTemplates" "E:\Dev\DAG\Global-Resources\ScriptTemplates"

if not exist "%ASSETTARGET%\Invector-FSMAIController\" goto invectortps
@echo Mapping Invector FSM scripts...
mklink /J "%ASSETTARGET%\Invector-FSMAIController\Scripts\AI\Custom" "E:\Dev\DAG\Global-Resources\Global-Scripts\Integrations\Invector\FSMAI"

:invectortps

if not exist "%ASSETTARGET%\Invector-3rdPersonController\" goto done
@echo Mapping Invector Shooter scripts...
mklink /J "%ASSETTARGET%\Invector-3rdPersonController\Shooter\Scripts\Custom" "E:\Dev\DAG\Global-Resources\Global-Scripts\Integrations\Invector\ThirdPersonShooter"

goto done

:foldernotexists
@echo Asset folder could not be found. Aborting.
goto end

:abort
@echo Aborted
goto end

:done
@echo Done creating script links

:end
pause
