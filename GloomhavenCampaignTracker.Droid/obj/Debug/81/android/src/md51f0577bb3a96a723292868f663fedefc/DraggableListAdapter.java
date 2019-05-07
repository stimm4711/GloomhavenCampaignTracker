package md51f0577bb3a96a723292868f663fedefc;


public abstract class DraggableListAdapter
	extends android.widget.BaseAdapter
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("GloomhavenCampaignTracker.Droid.CustomControls.DraggableListAdapter, Gloomhaven Campaign Tracker", DraggableListAdapter.class, __md_methods);
	}


	public DraggableListAdapter ()
	{
		super ();
		if (getClass () == DraggableListAdapter.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Droid.CustomControls.DraggableListAdapter, Gloomhaven Campaign Tracker", "", this, new java.lang.Object[] {  });
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
