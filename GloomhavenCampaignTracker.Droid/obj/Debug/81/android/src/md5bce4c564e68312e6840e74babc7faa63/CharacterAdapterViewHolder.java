package md5bce4c564e68312e6840e74babc7faa63;


public class CharacterAdapterViewHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("GloomhavenCampaignTracker.Droid.Adapter.CharacterAdapterViewHolder, Gloomhaven Campaign Tracker", CharacterAdapterViewHolder.class, __md_methods);
	}


	public CharacterAdapterViewHolder ()
	{
		super ();
		if (getClass () == CharacterAdapterViewHolder.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Droid.Adapter.CharacterAdapterViewHolder, Gloomhaven Campaign Tracker", "", this, new java.lang.Object[] {  });
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
