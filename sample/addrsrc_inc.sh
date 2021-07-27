# root_path='/home/username/workspace_git/dotunity' # Used in Linux, hardcode
root_path='E:/workspace_git/dotunity' # Used in Window, hardcode
root_name=${root_name:-$(basename $root_path)}

source $root_path/libinc.sh

path_sln=$root_path
name_sln=$root_name
path_out_dot=${path_out_dot:-$path_sln/out_dot}
path_out_u3d=${path_out_u3d:-$path_sln/out_u3d}
path_u3dprj=${path_u3dprj:-'E:/workspace_u3d/addrsrc'}
