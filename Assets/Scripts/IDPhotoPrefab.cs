using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDPhotoPrefab : MonoBehaviour
{
    // Base Canvas
    private Canvas canvas;
    private int sortOrder;

    // Sprite Objects
    SpriteRenderer eyes;
    SpriteRenderer nose;
    SpriteRenderer mouth;
    SpriteRenderer brows;
    SpriteRenderer bangs;
    SpriteRenderer hair;
    SpriteRenderer extension;
    SpriteRenderer clothes;
    SpriteRenderer body;

    // Clothes
    public Sprite polo;
    public Sprite shirt;

    // Eye Shapes
    public Sprite[] eyeSets;

    // Nose Shapes
    public Sprite[] noses;

    // Mouth Shapes
    public Sprite[] mouths;

    // Brow Shapes
    public Sprite[] browsDefault;
    public Sprite[] browsColored;

    // Bang Shapes
    public Sprite[] bangSets;

    // Short Hair
    public Sprite[] hairShort;

    // Long Hair
    public Sprite[] hairLong;

    // Hair Extensions
    public Sprite[] extensions;

    // Awake Function
    private void Awake()
    {
        eyes = transform.Find("eyes").GetComponent<SpriteRenderer>();
        nose = transform.Find("nose").GetComponent<SpriteRenderer>();
        mouth = transform.Find("mouth").GetComponent<SpriteRenderer>();
        brows = transform.Find("brows").GetComponent<SpriteRenderer>();
        bangs = transform.Find("bangs").GetComponent<SpriteRenderer>();
        hair = transform.Find("hair").GetComponent<SpriteRenderer>();
        extension = transform.Find("extension").GetComponent<SpriteRenderer>();
        clothes = transform.Find("clothes").GetComponent<SpriteRenderer>();
        body = transform.Find("body").GetComponent<SpriteRenderer>();
        canvas = GetComponent<Canvas>();
    }

    // Update function to maintain sorting order
    private void Update()
    {
        // Establish sorting order
        sortOrder = canvas.sortingOrder;

        eyes.sortingOrder = sortOrder + 1;
        nose.sortingOrder = sortOrder + 1;
        mouth.sortingOrder = sortOrder + 1;
        brows.sortingOrder = sortOrder + 1;
        bangs.sortingOrder = sortOrder + 1;
        hair.sortingOrder = sortOrder + 1;
        clothes.sortingOrder = sortOrder + 1;
        extension.sortingOrder = sortOrder;
        body.sortingOrder = sortOrder;
    }

    // Edit part of the ID Photo
    private void EditIDPhoto(SpriteRenderer sprite, Sprite newSprite)
    {
        sprite.sprite = newSprite;
    }

    // Generate Sprite based on customer information
    public void GenerateIDPhoto(Global.Customer customer, Canvas canvas)
    {
        // Declare variables for the sprites
        Sprite newEyes = null;
        Sprite newNose = null;
        Sprite newMouth = null;
        Sprite newBrows = null;
        Sprite newBangs = null;
        Sprite newHair = null;
        Sprite newExtension = null;
        Sprite newClothes = null;

        // Set clothes depending on type
        switch (Random.Range(0, 1))
        {
            // Polo
            case 0:
                newClothes = polo;
                break;

            // Shirt
            case 1:
                newClothes = shirt;
                break;
        }

        // Set eyes
        Debug.Log(customer.IDUsed.IDEyes.ToString());
        newEyes = eyeSets[customer.IDUsed.IDEyes];

        // Set nose
        Debug.Log(customer.IDUsed.IDNose.ToString());
        newNose = noses[customer.IDUsed.IDNose];

        // Set mouth
        Debug.Log(customer.IDUsed.IDMouth.ToString());
        newMouth = mouths[customer.Mouth];

        // Set bangs and extension
        newBangs = bangSets[customer.IDUsed.IDBangs];
        newExtension = extensions[customer.IDUsed.IDHairExtension];

        // Set hair depending on type
        if (customer.IDUsed.IDHairColor == 0)
        {
            // Default brows
            newBrows = browsDefault[customer.IDUsed.IDBrows];
        }
        else
        { 
            newBrows = browsColored[customer.IDUsed.IDBrows]; 
        }

        // Set hair length depending on sex
        switch (customer.Sex)
        {
            // Male
            case 0:
                newHair = hairShort[customer.IDUsed.IDHair];
                break;

            //Female
            case 1:
                newHair = hairLong[customer.IDUsed.IDHair];
                break;
        }

        // If bald, why bother?
        if (customer.IDUsed.IDHair == 0)
        {
            newBangs = bangSets[0];
            newExtension = extensions[0];
        }

        // If Male, why bangs?
        if (customer.Sex == 0)
        {
            newBangs = bangSets[0];
        }

        eyes.sprite = newEyes;
        nose.sprite = newNose;
        mouth.sprite = newMouth;
        brows.sprite = newBrows;
        bangs.sprite = newBangs;
        hair.sprite = newHair;
        extension.sprite = newExtension;
        clothes.sprite = newClothes;
    }

    // Make the subject a kid in the ID photo
    public void KidIDPhoto(int sex, int type)
    {
        // Declare variables for the sprites
        Sprite newEyes = null;
        Sprite newMouth = null;
        Sprite newBangs = null;
        Sprite newHair = null;
        Sprite newExtension = null;
        Sprite newClothes = null;

        // Set young features
        newClothes = polo;
        switch(type)
        {
            // Elementary
            case 5:
                newEyes = eyeSets[3];
                newMouth = mouths[0];
                break;

            // High School
            case 6:
                newEyes = eyeSets[10];
                newMouth = mouths[8];
                break;
        }

        switch(sex)
        {
            // Male
            case 0:
                newHair = hairShort[3];
                newBangs = bangSets[0];
                newExtension = extensions[0];
                break;

            // Female
            case 1:
                newHair = hairLong[4];
                newBangs = bangSets[0];
                newExtension = extensions[0];
                break;
        }

        eyes.sprite = newEyes; 
        mouth.sprite = newMouth;
        bangs.sprite = newBangs;
        hair.sprite = newHair;
        extension.sprite = newExtension;
        clothes.sprite = newClothes;
    }
}
