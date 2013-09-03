package xamarin.auth;


public class WebAuthenticatorActivity_State
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Xamarin.Auth.WebAuthenticatorActivity/State, Microsoft.WindowsAzure.MobileServices.Android, Version=1.0.4.0, Culture=neutral, PublicKeyToken=null", WebAuthenticatorActivity_State.class, __md_methods);
	}


	public WebAuthenticatorActivity_State ()
	{
		super ();
		if (getClass () == WebAuthenticatorActivity_State.class)
			mono.android.TypeManager.Activate ("Xamarin.Auth.WebAuthenticatorActivity/State, Microsoft.WindowsAzure.MobileServices.Android, Version=1.0.4.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
