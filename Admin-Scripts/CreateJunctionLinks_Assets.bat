@echo off
if "%~1" == "" goto getpipeline
if "%~2" == "" goto getpipeline
set ASSETTARGET=%1
set PIPELINE=%2
goto createlinks

:getpipeline
rem Determine pipeline
set "PIPELINE="
set /p "PIPELINE=Project render pipeline (B)RIP, (H)DRP, (U)RP: "
if "%PIPELINE%" == "B" goto gettarget
if "%PIPELINE%" == "H" goto gettarget
if "%PIPELINE%" == "U" goto gettarget
@echo Invalid pipeline entry.
goto getpipeline

:gettarget
set "ASSETTARGET="
set /p "ASSETTARGET=Project Assets path (empty to quit): "
if [%ASSETTARGET%]==[] goto abort
if exist "%ASSETTARGET%\" goto createlinks
@echo Project asset folder does not exist
goto gettarget

:createlinks
@echo Global asset links
@echo Clearing previous links...
rmdir "%ASSETTARGET%\Global-Assets" /S /Q
@echo Creating links...
mklink /J "%ASSETTARGET%\Global-Assets" "E:\Dev\DAG\Global-Resources\Global-Assets"

if "%PIPELINE%" == "B" goto createbirplinks
if "%PIPELINE%" == "H" goto createhdrplinks
if "%PIPELINE%" == "U" goto createurplinks

@echo No pipeline specified.
goto done

:createbirplinks
@echo BIRP pipeline asset links
@echo Clearing previous links...
rmdir "%ASSETTARGET%\Global-Assets-BIRP" /S /Q >nul 2>&1
del "%ASSETTARGET%\Global-Assets-BIRP.meta" /S >nul 2>&1
@echo Creating links...
mklink /J "%ASSETTARGET%\Global-Assets-BIRP" "E:\Dev\DAG\Global-Resources\Global-Assets-BIRP"
goto done

:createhdrplinks
@echo HDRP pipeline asset links
@echo Clearing previous links...
rmdir "%ASSETTARGET%\Global-Assets-HDRP" /S /Q >nul 2>&1
del "%ASSETTARGET%\Global-Assets-HDRP.meta" /S >nul 2>&1
@echo Creating links...
mklink /J "%ASSETTARGET%\Global-Assets-HDRP" "E:\Dev\DAG\Global-Resources\Global-Assets-HDRP"

goto done

:createurplinks
@echo URP pipeline asset links
@echo Clearing previous links...
rmdir "%ASSETTARGET%\Global-Assets-URP" /S /Q >nul 2>&1
del "%ASSETTARGET%\Global-Assets-URP.meta" /S >nul 2>&1
@echo Creating links...
mklink /J "%ASSETTARGET%\Global-Assets-URP" "E:\Dev\DAG\Global-Resources\Global-Assets-URP"

goto done

:foldernotexists
@echo Asset folder could not be found. Aborting.
goto end

:abort
@echo Aborted
goto end

:done
@echo Done creating asset links

:end
pause