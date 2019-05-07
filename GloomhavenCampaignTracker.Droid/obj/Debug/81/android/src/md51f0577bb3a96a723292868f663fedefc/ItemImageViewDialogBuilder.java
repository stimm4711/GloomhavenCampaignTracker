package md51f0577bb3a96a723292868f663fedefc;


public class ItemImageViewDialogBuilder
	extends android.app.AlertDialog.Builder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_setTitle:(Ljava/lang/CharSequence;)Landroid/app/AlertDialog$Builder;:GetSetTitle_Ljava_lang_CharSequence_Handler\n" +
			"n_setMessage:(I)Landroid/app/AlertDialog$Builder;:GetSetMessage_IHandler\n" +
			"n_setMessage:(Ljava/lang/CharSequence;)Landroid/app/AlertDialog$Builder;:GetSetMessage_Ljava_lang_CharSequence_Handler\n" +
			"n_setIcon:(I)Landroid/app/AlertDialog$Builder;:GetSetIcon_IHandler\n" +
			"n_setIcon:(Landroid/graphics/drawable/Drawable;)Landroid/app/AlertDialog$Builder;:GetSetIcon_Landroid_graphics_drawable_Drawable_Handler\n" +
			"n_show:()Landroid/app/AlertDialog;:GetShowHandler\n" +
			"";
		mono.android.Runtime.register ("GloomhavenCampaignTracker.Droid.CustomControls.ItemImageViewDialogBuilder, Gloomhaven Campaign Tracker", ItemImageViewDialogBuilder.class, __md_methods);
	}


	public ItemImageViewDialogBuilder (android.content.Context p0)
	{
		super (p0);
		if (getClass () == ItemImageViewDialogBuilder.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Droid.CustomControls.ItemImageViewDialogBuilder, Gloomhaven Campaign Tracker", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public ItemImageViewDialogBuilder (android.content.Context p0, int p1)
	{
		super (p0, p1);
		if (getClass () == ItemImageViewDialogBuilder.class)
			mono.android.TypeManager.Activate ("GloomhavenCampaignTracker.Droid.CustomControls.ItemImageViewDialogBuilder, Gloomhaven Campaign Tracker", "Android.Content.Context, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1 });
	}


	public android.app.AlertDialog.Builder setTitle (java.lang.CharSequence p0)
	{
		return n_setTitle (p0);
	}

	private native android.app.AlertDialog.Builder n_setTitle (java.lang.CharSequence p0);


	public android.app.AlertDialog.Builder setMessage (int p0)
	{
		return n_setMessage (p0);
	}

	private native android.app.AlertDialog.Builder n_setMessage (int p0);


	public android.app.AlertDialog.Builder setMessage (java.lang.CharSequence p0)
	{
		return n_setMessage (p0);
	}

	private native android.app.AlertDialog.Builder n_setMessage (java.lang.CharSequence p0);


	public android.app.AlertDialog.Builder setIcon (int p0)
	{
		return n_setIcon (p0);
	}

	private native android.app.AlertDialog.Builder n_setIcon (int p0);


	public android.app.AlertDialog.Builder setIcon (android.graphics.drawable.Drawable p0)
	{
		return n_setIcon (p0);
	}

	private native android.app.AlertDialog.Builder n_setIcon (android.graphics.drawable.Drawable p0);


	public android.app.AlertDialog show ()
	{
		return n_show ();
	}

	private native android.app.AlertDialog n_show ();

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
