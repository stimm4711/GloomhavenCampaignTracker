package md5d313efb64d2731be5af9ce0d29c7267a;


public class DL_Ability
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("GloomhavenCampaignTracker.Shared.Data.Entities.DL_Ability, Gloomhaven Campaign Tracker", DL_Ability.class, __md_methods);
	}


	public DL_Ability ()
	{
		super ();
		if (getClass () == DL_Ability.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Shared.Data.Entities.DL_Ability, Gloomhaven Campaign Tracker", "", this, new java.lang.Object[] {  });
	}

	private java.util.ArrayList refList;
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
