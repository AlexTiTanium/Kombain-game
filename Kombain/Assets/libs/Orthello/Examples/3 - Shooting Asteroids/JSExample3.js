// ------------------------------------------------------------------------
// Orthello 2D Framework Example 
// (C)opyright 2011 - WyrmTale Games - http://www.wyrmtale.com
// ------------------------------------------------------------------------
// More info http://www.wyrmtale.com/orthello
// ------------------------------------------------------------------------
// Because Orthello is created as a C# framework the C# classes 
// will only be available as you place them in the /Standard Assets folder.
//
// If you would like to test the JS examples or use the framework in combination
// with Javascript coding, you will have to move the /Orthello/Standard Assets folder
// to the / (root).
//
// This code was commented to prevent compiling errors when project is
// downloaded and imported using a package.
// ------------------------------------------------------------------------
// Main Example 3 Demo class
// ------------------------------------------------------------------------

/*

// sprite prototypes that will be used when creating objects
private var bullet:OTSprite;
// the asteroid prototypes will be de-activated on start
private var a1:OTAnimatingSprite;
private var a2:OTAnimatingSprite;
private var a3:OTAnimatingSprite;


private var gun:OTAnimatingSprite;              // gun sprite reference
private var initialized:boolean = false;        // initialization notifier


function Awake()
{
    // keep our prototypes by de-activating them.
	a1 = GameObject.Find("asteroid 1").GetComponent("OTAnimatingSprite");
	a2 = GameObject.Find("asteroid 2").GetComponent("OTAnimatingSprite");
	a3 = GameObject.Find("asteroid 3").GetComponent("OTAnimatingSprite");
	bullet = GameObject.Find("bullet").GetComponent("OTSprite");
	if (bullet!=null) bullet.gameObject.active = false;

    a1.gameObject.active = false;
    a2.gameObject.active = false;
    a3.gameObject.active = false;
    // By de-acivating a gameobject is becomes invisible but can still
    // be used them to instantiate copies.
}

// This method will create an asteroid at a random position on screen and with
// relative min/max (0-1) size. An OTObject can be provided to act as a base to 
// determine the new size.
private var dp:int = 0;
function RandomBlock(r:Rect, min:Number, max:Number, o:OTObject)
{
    // Determine random 1-3 asteroid type
    var t:int = 1 + Mathf.Floor(Random.value * 3);
    // Determine random size modifier (min-max)
    var s:Number = min + Random.value * (max - min);
    var g:GameObject = null;
    // Create a new asteroid
        switch (t)
        {
            case 1: g = OT.CreateObject("asteroid1");
                break;
            case 2: g = OT.CreateObject("asteroid2");
                break;
            case 3: g = OT.CreateObject("asteroid3");
                break;
        }
    if (g != null)
    {
        // Find this new asteroid's animating sprite
        var sprite:OTAnimatingSprite = g.GetComponent("OTAnimatingSprite");
        // If a base object was provided use it for size scaling
        if (o != null)
            sprite.size = o.size * s;
        else
            sprite.size = sprite.size * s;
        // Set sprite's random position
        sprite.position = new Vector2(r.xMin + Random.value * r.width, r.yMin + Random.value * r.height);
        // Set sprote's random rotation
        sprite.rotation = Random.value * 360;
        // Set sprite's name
        sprite.depth = dp++;
        if (dp > 750) dp = 100;
		
        // Return new sprite
        return sprite;
    }
    // we did not manage to create a sprite/asteroid
    return null;
}


// Create objects for this application
function CreateObjectPools()
{
	OT.PreFabricate("asteroid1",100);
	OT.PreFabricate("asteroid2",100);
	OT.PreFabricate("asteroid3",100);		
}

// application initialization
function Initialize()
{
    // Get reference to gun animation sprite
    gun = OT.ObjectByName("gun") as OTAnimatingSprite;
	// Because Javascript does not support C# delegate we have to notify our sprite class that 
	// we want to receive notification callbacks.
	// If we have initialized for callbacks our (!IMPORTANT->) 'public' declared call back 
	// functions will be asutomaticly called when an event takes place.
	// HINT : This technique can be used within a C# class as well.
    gun.InitCallBacks(this);
    // Create our object pool if we want.
    OT.objectPooling = true;
    if (OT.objectPooling)
        CreateObjectPools();
    // set our initialization notifier - we only want to initialize once
    initialized = true;
}

// This method will explode an asteroid
public function Explode(o:OTObject, bullet:JSBullet3)
{
    // Determine how many debree has to be be created
    var blocks:int = 2 + Mathf.Floor(Random.value * 2);
    // Notify that this asteroid has to be destroyed
    OT.DestroyObject(o);
    // Create debree
    for (var b:int = 0; b < blocks; b++)
    {
        // Shrink asteroid's rect to act as the random position container
        // for the debree
        var r:Rect = new Rect(
            o.rect.x + o.rect.width / 4,
            o.rect.y + o.rect.height / 4,
            o.rect.width / 2,
            o.rect.height / 2);
        // Create a debree that is relatively smaller than the asteroid that was detroyed
        var a:OTAnimatingSprite = RandomBlock(r, 0.6f, 0.75f, o);
        // Add this debree to the bullet telling the bullet to ignore this debree
        // in this update cycle - otherwise the bullet explosions could enter some
        // recursive 'dead' loop creating LOTS of debree
        bullet.AddDebree(a);
        // Recusively explode 2 asteroids if they are big enough, to get a nice
        // exploding debree effect.
        if (b < 2 && a.size.x > 30)
            Explode(a, bullet);
    }
}

// Update is called once per frame
function Update () {
    // only go one if Orthello is initialized
    if (!OT.isValid) return;

    // We call the application initialization function from Update() once 
    // because we want to be sure that all Orthello objects have been started.
    if (!initialized)
	{
        Initialize();
		return;
	}

    // Rotate the gun animation sprite towards the mouse on screen
    gun.RotateTowards(OT.view.mouseWorldPosition);
    // Rotate our bullet prototype as well so we will instantiate a
    // 'already rotated' bullet when we shoot
    bullet.rotation = gun.rotation;

    // check if the left mouse button was clicked
    if (Input.GetMouseButtonDown(0))
    {
        // Create a new bullet
        var nBullet:OTSprite = (Instantiate(bullet.gameObject) as GameObject).GetComponent("OTSprite");
        // Set bullet's position at approximately the gun's shooting barrel
        nBullet.position = gun.position + gun.transform.up * (gun.size.y / 2);
        // Play the gun's shooting animation frameset once
        gun.PlayOnce("shoot");
    }

    // If we have less than 15 objects within Orthello we will create a random asteroid
    if (OT.objectCount <= 12)
        RandomBlock(OT.view.worldRect, 0.6f, 1.2f, null);        
}

// The OnAnimationFinish delegate will be called when an animation or animation frameset
// finishes playing.
public function OnAnimationFinish(owner:OTObject)
{
    // Because the only animation that finishes will be the gun's 'shoot' animation frameset
    // we know that we have to switch to the gun's looping 'idle' animation frameset
	if (owner == gun)	
		gun.PlayLoop("idle");
}

*/