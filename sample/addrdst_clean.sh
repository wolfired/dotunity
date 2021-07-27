#!/bin/env bash

source $(dirname $0)/addrdst_inc.sh



#---------------------------------------------------------------
DotSolutionClean $path_sln $path_out_dot

name_dotprj=u3dcsprojcloner
DotProjectClean $path_sln/$name_dotprj

name_dotprj=nopack_depend_player
DotProjectClean $path_sln/$name_dotprj

name_dotprj=nopack_depend_editor
DotProjectClean $path_sln/$name_dotprj

name_dotprj=pack_depend_player
DotProjectClean $path_sln/$name_dotprj

name_dotprj=pack_depend_editor
DotProjectClean $path_sln/$name_dotprj
