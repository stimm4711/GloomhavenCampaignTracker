package md5aff4aeb270111e744e14f3a0744c9f94;


public class EventWrapper
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("GloomhavenCampaignTracker.Droid.Fragments.campaign.EventWrapper, Gloomhaven Campaign Tracker", EventWrapper.class, __md_methods);
	}


	public EventWrapper ()
	{
		super ();
		if (getClass () == EventWrapper.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Droid.Fragments.campaign.EventWrapper, Gloomhaven Campaign Tracker", "", this, new java.lang.Object[] {  });
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
