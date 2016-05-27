package md5ac175dc26e86eacab836e0c6a73ed69f;


public class NewProfile
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("CoffeeApp.NewProfile, CoffeeApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", NewProfile.class, __md_methods);
	}


	public NewProfile () throws java.lang.Throwable
	{
		super ();
		if (getClass () == NewProfile.class)
			mono.android.TypeManager.Activate ("CoffeeApp.NewProfile, CoffeeApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
