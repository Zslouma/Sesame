using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Syracuse.Mobitheque.Core.PermissionsManagement
{
    public static class CheckAndRequestPermissions
    {
            public static async Task<PermissionStatus> CheckAndRequestLocationPermission()
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (status == PermissionStatus.Granted)
                    return status;

                if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    // Prompt the user to turn on in settings
                    // On iOS once a permission has been denied it may not be requested again from the application
                    return status;
                }

                //if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
                //{
                //    // Prompt the user with additional information as to why the permission is needed
                //}

                status = await Permissions.RequestAsync<Permissions.Camera>();

                return status;
            }
        }
    }

