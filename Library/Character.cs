using Squaddie.Property;
using Squaddie.Properties;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Squaddie
{
    public sealed class Character
    {
        public List<IProperty> Properties { get; set; }

        public Character()
        {
            Properties = new List<IProperty>();
            InitializeDefault();
        }

        public void InitializeDefault()
        {
            PropertyFactory factory = new PropertyFactory();

            AddOrUpdateProperty(factory.CreateProperty("strFirstName", StringProperty.TypeName, "First"));
            AddOrUpdateProperty(factory.CreateProperty("strLastName", StringProperty.TypeName, "Last"));
            AddOrUpdateProperty(factory.CreateProperty("strNickName", StringProperty.TypeName, ""));
            AddOrUpdateProperty(factory.CreateProperty("m_SoldierClassTemplateName", NameProperty.TypeName, "Rookie"));
            AddOrUpdateProperty(factory.CreateProperty("CharacterTemplateName", NameProperty.TypeName, "Soldier"));

            //Appearance List
            List<IProperty> kAppearance = new List<IProperty>();
            kAppearance.Add(factory.CreateProperty("nmHead", NameProperty.TypeName, "CaucMale_A"));
            kAppearance.Add(factory.CreateProperty("iGender", IntProperty.TypeName, 1));
            kAppearance.Add(factory.CreateProperty("iRace", IntProperty.TypeName, 0));
            kAppearance.Add(factory.CreateProperty("nmHaircut", NameProperty.TypeName, "MaleHair_Blank"));
            kAppearance.Add(factory.CreateProperty("iHairColor", IntProperty.TypeName, 0));
            kAppearance.Add(factory.CreateProperty("iFacialHair", IntProperty.TypeName, 0));
            kAppearance.Add(factory.CreateProperty("nmBeard", NameProperty.TypeName, "MaleBeard_Blank"));
            kAppearance.Add(factory.CreateProperty("iSkinColor", IntProperty.TypeName, 0));
            kAppearance.Add(factory.CreateProperty("iEyeColor", IntProperty.TypeName, 0));
            kAppearance.Add(factory.CreateProperty("nmFlag", NameProperty.TypeName, "Country_UK"));
            kAppearance.Add(factory.CreateProperty("iVoice", IntProperty.TypeName, 0));
            kAppearance.Add(factory.CreateProperty("iAttitude", IntProperty.TypeName, 2));
            kAppearance.Add(factory.CreateProperty("iArmorDeco", IntProperty.TypeName, 0));
            kAppearance.Add(factory.CreateProperty("iArmorTint", IntProperty.TypeName, 0));
            kAppearance.Add(factory.CreateProperty("iArmorTintSecondary", IntProperty.TypeName, 0));
            kAppearance.Add(factory.CreateProperty("iWeaponTint", IntProperty.TypeName, 0));
            kAppearance.Add(factory.CreateProperty("iTattooTint", IntProperty.TypeName, 0));
            kAppearance.Add(factory.CreateProperty("nmWeaponPattern", NameProperty.TypeName, "Pat_Nothing"));
            kAppearance.Add(factory.CreateProperty("nmPawn", NameProperty.TypeName, "None"));
            kAppearance.Add(factory.CreateProperty("nmTorso", NameProperty.TypeName, "CnvMed_Std_A_M"));
            kAppearance.Add(factory.CreateProperty("nmArms", NameProperty.TypeName, "CnvMed_Std_A_M"));
            kAppearance.Add(factory.CreateProperty("nmLegs", NameProperty.TypeName, "CnvMed_Std_A_M"));
            kAppearance.Add(factory.CreateProperty("nmHelmet", NameProperty.TypeName, "Helmet_0_NoHelmet_M"));
            kAppearance.Add(factory.CreateProperty("nmEye", NameProperty.TypeName, "DefaultEyes"));
            kAppearance.Add(factory.CreateProperty("nmTeeth", NameProperty.TypeName, "DefaultTeeth"));
            kAppearance.Add(factory.CreateProperty("nmFacePropLower", NameProperty.TypeName, "Prop_FaceLower_Blank"));
            kAppearance.Add(factory.CreateProperty("nmFacePropUpper", NameProperty.TypeName, "Prop_FaceUpper_Blank"));
            kAppearance.Add(factory.CreateProperty("nmPatterns", NameProperty.TypeName, "Pat_Nothing"));
            kAppearance.Add(factory.CreateProperty("nmVoice", NameProperty.TypeName, "MaleVoice1_English_UK"));
            kAppearance.Add(factory.CreateProperty("nmLanguage", NameProperty.TypeName, "None"));
            kAppearance.Add(factory.CreateProperty("nmTattoo_LeftArm", NameProperty.TypeName, "Tattoo_Arms_BLANK"));
            kAppearance.Add(factory.CreateProperty("nm_Tattoo_RightArm", NameProperty.TypeName, "Tattoo_Arms_BLANK"));
            kAppearance.Add(factory.CreateProperty("nmScards", NameProperty.TypeName, "None"));
            kAppearance.Add(factory.CreateProperty("nmTorso_Underlay", NameProperty.TypeName, "CnvUnderlay_Std_Torsos_A_M"));
            kAppearance.Add(factory.CreateProperty("nmArms_Underlay", NameProperty.TypeName, "CnvUnderlay_Std_Arms_A_M"));
            kAppearance.Add(factory.CreateProperty("nmLegs_Underlay", NameProperty.TypeName, "CnvUnderlay_Std_Legs_A_M"));
            kAppearance.Add(factory.CreateProperty("nmFacePaint", NameProperty.TypeName, "None"));
            kAppearance.Add(factory.CreateProperty("nmLeftArm", NameProperty.TypeName, "None"));
            kAppearance.Add(factory.CreateProperty("nmRightArm", NameProperty.TypeName, "None"));
            kAppearance.Add(factory.CreateProperty("nmLeftArmDeco", NameProperty.TypeName, "None"));
            kAppearance.Add(factory.CreateProperty("nmRightArmDeco", NameProperty.TypeName, "None"));
            kAppearance.Add(factory.CreateProperty("nmLeftForearm", NameProperty.TypeName, "None"));
            kAppearance.Add(factory.CreateProperty("nmRightForearm", NameProperty.TypeName, "None"));
            kAppearance.Add(factory.CreateProperty("nmThighs", NameProperty.TypeName, "None"));
            kAppearance.Add(factory.CreateProperty("nmShins", NameProperty.TypeName, "None"));
            kAppearance.Add(factory.CreateProperty("nmTorsoDeco", NameProperty.TypeName, "None"));
            kAppearance.Add(factory.CreateProperty("bGhostPawn", BoolProperty.TypeName, false));

            AddOrUpdateProperty(factory.CreateProperty("kAppearance", StructProperty.TypeName, kAppearance));

            AddOrUpdateProperty(factory.CreateProperty("Country", NameProperty.TypeName, "Country_UK"));
            AddOrUpdateProperty(factory.CreateProperty("AllowedTypeSoldier", BoolProperty.TypeName, true));
            AddOrUpdateProperty(factory.CreateProperty("AllowedTypeVIP", BoolProperty.TypeName, false));
            AddOrUpdateProperty(factory.CreateProperty("AllowedTypeDarkVIP", BoolProperty.TypeName, false));
            AddOrUpdateProperty(factory.CreateProperty("PoolTimestamp", StringProperty.TypeName, "February 5, 2016 - 00:01"));
            AddOrUpdateProperty(factory.CreateProperty("BackgroundText", StringProperty.TypeName, "Created using the Squaddie Character Pool Parser."));
        }

        public void AddOrUpdateProperty(IProperty item)
        {
            //If the property already exists with the same name and type, then we should just change the value
            IProperty prop = Properties.Find(x => x.Name == item.Name && x.Type == item.Type);
            if (prop != null)
            {
                switch (item.Type)
                {
                    case ArrayProperty.TypeName:
                        ((ArrayProperty)prop).Value = ((ArrayProperty)item).Value;
                        break;
                    case IntProperty.TypeName:
                        ((IntProperty)prop).Value = ((IntProperty)item).Value;
                        break;
                    case BoolProperty.TypeName:
                        ((BoolProperty)prop).Value = ((BoolProperty)item).Value;
                        break;
                    case NameProperty.TypeName:
                        ((NameProperty)prop).Value = ((NameProperty)item).Value;
                        break;
                    case StringProperty.TypeName:
                        ((StringProperty)prop).Value = ((StringProperty)item).Value;
                        break;
                    case StructProperty.TypeName:
                        ((StructProperty)prop).Value = ((StructProperty)item).Value;
                        break;
                    default:
                        throw new Exception("Property Amend Error: Cannot edit property named " + item.Name + " and of type " + item.Type + ".");
                }
            }
            else
            {
                Properties.Add(item);
            }
        }
    }
}
