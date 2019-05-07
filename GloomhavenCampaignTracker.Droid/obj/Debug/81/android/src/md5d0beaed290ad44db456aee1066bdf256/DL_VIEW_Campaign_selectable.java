package md5d0beaed290ad44db456aee1066bdf256;


public class DL_VIEW_Campaign_selectable
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Data.ViewEntities.DL_VIEW_Campaign_selectable, Gloomhaven Campaign Tracker", DL_VIEW_Campaign_selectable.class, __md_methods);
	}


	public DL_VIEW_Campaign_selectable ()
	{
		super ();
		if (getClass () == DL_VIEW_Campaign_selectable.class)
			mono.android.TypeManager.Activate ("Data.ViewEntities.DL_VIEW_Campaign_selectable, Gloomhaven Campaign Tracker", "", this, new java.lang.Object[] {  });
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
