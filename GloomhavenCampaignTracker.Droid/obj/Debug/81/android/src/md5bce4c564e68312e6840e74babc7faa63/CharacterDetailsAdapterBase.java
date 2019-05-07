package md5bce4c564e68312e6840e74babc7faa63;


public abstract class CharacterDetailsAdapterBase
	extends android.widget.BaseAdapter
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getItemId:(I)J:GetGetItemId_IHandler\n" +
			"n_getItem:(I)Ljava/lang/Object;:GetGetItem_IHandler\n" +
			"";
		mono.android.Runtime.register ("GloomhavenCampaignTracker.Droid.Adapter.CharacterDetailsAdapterBase, Gloomhaven Campaign Tracker", CharacterDetailsAdapterBase.class, __md_methods);
	}


	public CharacterDetailsAdapterBase ()
	{
		super ();
		if (getClass () == CharacterDetailsAdapterBase.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Droid.Adapter.CharacterDetailsAdapterBase, Gloomhaven Campaign Tracker", "", this, new java.lang.Object[] {  });
	}


	public long getItemId (int p0)
	{
		return n_getItemId (p0);
	}

	private native long n_getItemId (int p0);


	public java.lang.Object getItem (int p0)
	{
		return n_getItem (p0);
	}

	private native java.lang.Object n_getItem (int p0);

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
