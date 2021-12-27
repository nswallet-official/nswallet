set -e
set -x
FILE_APP_BUILD=/Users/devops/bitrise_build_numbers/nswallet_build_number
LATEST_APP_BUILD=$(cat $FILE_APP_BUILD)
envman add --key BITRISE_PREVIOUS_BUILD_NUMBER --value $LATEST_APP_BUILD
NEW_APP_BUILD=$(($LATEST_APP_BUILD + 1))
echo $NEW_APP_BUILD > $FILE_APP_BUILD
envman add --key BITRISE_BUILD_NUMBER --value $NEW_APP_BUILD
