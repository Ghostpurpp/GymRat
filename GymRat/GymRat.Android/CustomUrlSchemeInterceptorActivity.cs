using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace GymRat.Droid
{
    [Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
	[IntentFilter(
		new[] { Intent.ActionView },
		Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
		DataSchemes = new[] { "com.googleusercontent.apps.923270163248-st1kjui4qkjda0dbmu7ads3fffk7n9fb" },
		DataPath = "/oauth2redirect")]
	public class CustomUrlSchemeInterceptorActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Convert Android.Net.Url to Uri
			global::Android.Net.Uri uri_android = Intent.Data;

			Uri uri_netfx = new Uri(uri_android.ToString());

			// load redirect_url Page
			AuthenticationState.Authenticator.OnPageLoading(uri_netfx);

			var intent = new Intent(this, typeof(MainActivity));
			intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
			StartActivity(intent);

			this.Finish();

			return;
		}
	}
}