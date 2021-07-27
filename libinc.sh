root_path=${root_path:?"need dotunity project root path"}
root_name=${root_name:-$(basename $root_path)}

source $root_path/libdot.sh
source $root_path/libunity.sh
source $root_path/libother.sh
