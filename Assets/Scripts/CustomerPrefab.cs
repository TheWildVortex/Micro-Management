using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerPrefab : MonoBehaviour
{
    // Sprite Objects
    private int sortNumber;
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
    public Sprite[] clothesPolo;
    public Sprite[] clothesSando;
    public Sprite[] clothesShirt;
    public Sprite[] clothesRolled;
    public Sprite[] clothesSleeve;

    // Skin Bases
    public Sprite[] skinX;
    public Sprite[] skinY;

    // Eye Shapes
    public Sprite[] eyesA;
    public Sprite[] eyesB;
    public Sprite[] eyesC;
    public Sprite[] eyesD;
    public Sprite[] eyesE;

    // Nose Shapes
    public Sprite[] noses;

    // Mouth Shapes
    public Sprite[] mouthsX;
    public Sprite[] mouthsY;

    // Brow Shapes
    public Sprite[] browsA;
    public Sprite[] browsB;
    public Sprite[] browsC;
    public Sprite[] browsD;
    public Sprite[] browsE;

    // Bang Shapes
    public Sprite[] bangsA;
    public Sprite[] bangsB;
    public Sprite[] bangsC;
    public Sprite[] bangsD;
    public Sprite[] bangsE;

    // Short Hair
    public Sprite[] hairShortA;
    public Sprite[] hairShortB;
    public Sprite[] hairShortC;
    public Sprite[] hairShortD;
    public Sprite[] hairShortE;

    // Long Hair
    public Sprite[] hairLongA;
    public Sprite[] hairLongB;
    public Sprite[] hairLongC;
    public Sprite[] hairLongD;
    public Sprite[] hairLongE;

    // Hair Extensions
    public Sprite[] extensionA;
    public Sprite[] extensionB;
    public Sprite[] extensionC;
    public Sprite[] extensionD;
    public Sprite[] extensionE;

    // Awake Function
    private void Awake()
    {
        sortNumber = GetComponent<Canvas>().sortingOrder;
        Debug.Log("Customer Sort Number: " + sortNumber.ToString());
        eyes = transform.Find("eyes").GetComponent<SpriteRenderer>();
        nose = transform.Find("nose").GetComponent<SpriteRenderer>();
        mouth = transform.Find("mouth").GetComponent<SpriteRenderer>();
        brows = transform.Find("brows").GetComponent<SpriteRenderer>();
        bangs = transform.Find("bangs").GetComponent<SpriteRenderer>();
        hair = transform.Find("hair").GetComponent<SpriteRenderer>();
        extension = transform.Find("extension").GetComponent<SpriteRenderer>();
        clothes = transform.Find("clothes").GetComponent<SpriteRenderer>();
        body = transform.Find("body").GetComponent<SpriteRenderer>();
    }

    // Maintain correct sorting order
    private void Update()
    {
        eyes.sortingOrder = nose.sortingOrder = mouth.sortingOrder = brows.sortingOrder = bangs.sortingOrder = hair.sortingOrder = clothes.sortingOrder = sortNumber + 2;
        body.sortingOrder = extension.sortingOrder = sortNumber + 1;
    }

    // Generate Sprite based on customer information
    public void GenerateSprite(Global.Customer customer, Canvas canvas)
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
        Sprite newBody = null;

        // Set clothes depending on type
        switch (customer.Clothes)
        {
            // Polo
            case 0:
                newClothes = clothesPolo[customer.ClothesColor];
                break;

            // Sando
            case 1:
                newClothes = clothesSando[customer.ClothesColor];
                break;

            // Shirt
            case 2:
                newClothes = clothesShirt[customer.ClothesColor];
                break;

            // Rolled
            case 3:
                newClothes = clothesRolled[customer.ClothesColor];
                break;

            // Sleeve
            case 4:
                newClothes = clothesSleeve[customer.ClothesColor];
                break;
        }

        // Set features depending on skin type
        switch (customer.SkinType)
        {
            // Skin X
            case 0:
                newBody = skinX[customer.BodyColor];
                newMouth = mouthsX[customer.Mouth];
                break;

            // Skin Y
            case 1:
                newBody = skinY[customer.BodyColor];
                newMouth = mouthsY[customer.Mouth];
                break;
        }

        // Set eyes depending on color
        switch (customer.EyeColor)
        {
            // A color eyes
            case 0:
                newEyes = eyesA[customer.Eyes];
                break;

            // B color eyes
            case 1:
                newEyes = eyesB[customer.Eyes];
                break;

            // C color eyes
            case 2:
                newEyes = eyesC[customer.Eyes];
                break;

            // D color eyes
            case 3:
                newEyes = eyesD[customer.Eyes];
                break;

            // E color eyes
            case 4:
                newEyes = eyesE[customer.Eyes];
                break;
        }

        // Set nose
        newNose = noses[customer.Nose];

        // Set hair depending on color
        switch (customer.HairColor)
        {
            // A color hair
            case 0:
                newBrows = browsA[customer.Brows];
                newBangs = bangsA[customer.Bangs];
                newExtension = extensionA[customer.HairExtension];

                // Set hair length depending on sex
                switch (customer.Sex)
                {
                    // Male
                    case 0:
                        newHair = hairShortA[customer.Hair];
                        break;

                    //Female
                    case 1:
                        newHair = hairLongA[customer.Hair];
                        break;
                }
                break;

            // B color hair
            case 1:
                newBrows = browsB[customer.Brows];
                newBangs = bangsB[customer.Bangs];
                newExtension = extensionB[customer.HairExtension];

                // Set hair length depending on sex
                switch (customer.Sex)
                {
                    // Male
                    case 0:
                        newHair = hairShortB[customer.Hair];
                        break;

                    //Female
                    case 1:
                        newHair = hairLongB[customer.Hair];
                        break;
                }
                break;

            // C color hair
            case 2:
                newBrows = browsC[customer.Brows];
                newBangs = bangsC[customer.Bangs];
                newExtension = extensionC[customer.HairExtension];

                // Set hair length depending on sex
                switch (customer.Sex)
                {
                    // Male
                    case 0:
                        newHair = hairShortC[customer.Hair];
                        break;

                    //Female
                    case 1:
                        newHair = hairLongC[customer.Hair];
                        break;
                }
                break;

            // D color hair
            case 3:
                newBrows = browsD[customer.Brows];
                newBangs = bangsD[customer.Bangs];
                newExtension = extensionD[customer.HairExtension];

                // Set hair length depending on sex
                switch (customer.Sex)
                {
                    // Male
                    case 0:
                        newHair = hairShortD[customer.Hair];
                        break;

                    //Female
                    case 1:
                        newHair = hairLongD[customer.Hair];
                        break;
                }
                break;

            // E color hair
            case 4:
                newBrows = browsE[customer.Brows];
                newBangs = bangsE[customer.Bangs];
                newExtension = extensionE[customer.HairExtension];

                // Set hair length depending on sex
                switch (customer.Sex)
                {
                    // Male
                    case 0:
                        newHair = hairShortE[customer.Hair];
                        break;

                    //Female
                    case 1:
                        newHair = hairLongE[customer.Hair];
                        break;
                }
                break;
        }

        // If bald, why bother?
        if (customer.Hair == 0)
        {
            newExtension = extensionA[0];
        }

        // If Male, why bangs?
        if (customer.Sex == 0)
        {
            newBangs = bangsA[0];
        }

        eyes.sprite = newEyes;
        nose.sprite = newNose;
        mouth.sprite = newMouth;
        brows.sprite = newBrows;
        bangs.sprite = newBangs;
        hair.sprite = newHair;
        extension.sprite = newExtension;
        clothes.sprite = newClothes;
        body.sprite = newBody;
    }
}
