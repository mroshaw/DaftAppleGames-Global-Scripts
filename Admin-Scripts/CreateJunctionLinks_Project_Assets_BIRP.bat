@echo off
@echo Junction Link Creator for BIRP
@echo *** BIRP ONLY ***

set /p "ASSETTARGET=Target Assets path: "

@echo Clearing previous links
rmdir "%ASSETTARGET%\Pipeline-Assets" /S /Q

@echo Mapping Pipeline HDRP Shared Assets
mklink /J "%ASSETTARGET%\Pipeline-Assets" "E:\Dev\DAG\Darskerry\BIRP_2022\Pipeline-Assets"

pause
