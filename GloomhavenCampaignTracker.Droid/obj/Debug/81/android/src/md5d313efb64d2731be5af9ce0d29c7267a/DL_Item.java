package md5d313efb64d2731be5af9ce0d29c7267a;


public class DL_Item
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_hashCode:()I:GetGetHashCodeHandler\n" +
			"";
		mono.android.Runtime.register ("GloomhavenCampaignTracker.Shared.Data.Entities.DL_Item, Gloomhaven Campaign Tracker", DL_Item.class, __md_methods);
	}


	public DL_Item ()
	{
		super ();
		if (getClass () == DL_Item.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Shared.Data.Entities.DL_Item, Gloomhaven Campaign Tracker", "", this, new java.lang.Object[] {  });
	}


	public int hashCode ()
	{
		return n_hashCode ();
	}

	private native int n_hashCode ();

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
