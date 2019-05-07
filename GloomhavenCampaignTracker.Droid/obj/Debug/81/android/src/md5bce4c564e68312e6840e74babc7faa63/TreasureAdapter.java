package md5bce4c564e68312e6840e74babc7faa63;


public class TreasureAdapter
	extends android.widget.BaseAdapter
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getCount:()I:GetGetCountHandler\n" +
			"n_getItemId:(I)J:GetGetItemId_IHandler\n" +
			"n_getView:(ILandroid/view/View;Landroid/view/ViewGroup;)Landroid/view/View;:GetGetView_ILandroid_view_View_Landroid_view_ViewGroup_Handler\n" +
			"n_getItem:(I)Ljava/lang/Object;:GetGetItem_IHandler\n" +
			"";
		mono.android.Runtime.register ("GloomhavenCampaignTracker.Droid.Adapter.TreasureAdapter, Gloomhaven Campaign Tracker", TreasureAdapter.class, __md_methods);
	}


	public TreasureAdapter ()
	{
		super ();
		if (getClass () == TreasureAdapter.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Droid.Adapter.TreasureAdapter, Gloomhaven Campaign Tracker", "", this, new java.lang.Object[] {  });
	}

	public TreasureAdapter (android.content.Context p0, md5e19c8c6d908ac58216f15880adf1d323.CampaignUnlockedScenario p1, md5bce4c564e68312e6840e74babc7faa63.ExpandableScenarioListAdapter p2)
	{
		super ();
		if (getClass () == TreasureAdapter.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Droid.Adapter.TreasureAdapter, Gloomhaven Campaign Tracker", "Android.Content.Context, Mono.Android:GloomhavenCampaignTracker.Business.CampaignUnlockedScenario, Gloomhaven Campaign Tracker:GloomhavenCampaignTracker.Droid.Adapter.ExpandableScenarioListAdapter, Gloomhaven Campaign Tracker", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public int getCount ()
	{
		return n_getCount ();
	}

	private native int n_getCount ();


	public long getItemId (int p0)
	{
		return n_getItemId (p0);
	}

	private native long n_getItemId (int p0);


	public android.view.View getView (int p0, android.view.View p1, android.view.ViewGroup p2)
	{
		return n_getView (p0, p1, p2);
	}

	private native android.view.View n_getView (int p0, android.view.View p1, android.view.ViewGroup p2);


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
