using Squaddie.Properties;
using System;
using System.Collections.Generic;

namespace Squaddie
{
    public sealed class Character : List<IProperty>
    {
        public Character()
        {
            InitializeDefault();
        }

        private void InitializeDefault()
        {
            Add(new Property("strFirstName", "StrProperty", "First"));
            Add(new Property("strLastName", "StrProperty", "Last"));
            Add(new Property("strNickName", "StrProperty", ""));
            Add(new Property("m_SoldierClassTemplateName", "NameProperty", "Rookie"));
            Add(new Property("CharacterTemplateName", "NameProperty", "Soldier"));

            //Appearance List
            List<IProperty> kAppearance = new List<IProperty>();
            kAppearance.Add(new Property("nmHead", "NameProperty", "CaucMale_A"));
            kAppearance.Add(new Property("iGender", "IntProperty", 1));
            kAppearance.Add(new Property("iRace", "IntProperty", 0));
            kAppearance.Add(new Property("nmHaircut", "NameProperty", "MaleHair_Blank"));
            kAppearance.Add(new Property("iHairColor", "IntProperty", 0));
            kAppearance.Add(new Property("iFacialHair", "IntProperty", 0));
            kAppearance.Add(new Property("nmBeard", "NameProperty", "MaleBeard_Blank"));
            kAppearance.Add(new Property("iSkinColor", "IntProperty", 0));
            kAppearance.Add(new Property("iEyeColor", "IntProperty", 0));
            kAppearance.Add(new Property("nmFlag", "NameProperty", "Country_UK"));
            kAppearance.Add(new Property("iVoice", "IntProperty", 0));
            kAppearance.Add(new Property("iAttitude", "IntProperty", 2));
            kAppearance.Add(new Property("iArmorDeco", "IntProperty", 0));
            kAppearance.Add(new Property("iArmorTint", "IntProperty", 0));
            kAppearance.Add(new Property("iArmorTintSecondary", "IntProperty", 0));
            kAppearance.Add(new Property("iWeaponTint", "IntProperty", 0));
            kAppearance.Add(new Property("iTattooTint", "IntProperty", 0));
            kAppearance.Add(new Property("nmWeaponPattern", "NameProperty", "Pat_Nothing"));
            kAppearance.Add(new Property("nmPawn", "NameProperty", "None"));
            kAppearance.Add(new Property("nmTorso", "NameProperty", "CnvMed_Std_A_M"));
            kAppearance.Add(new Property("nmArms", "NameProperty", "CnvMed_Std_A_M"));
            kAppearance.Add(new Property("nmLegs", "NameProperty", "CnvMed_Std_A_M"));
            kAppearance.Add(new Property("nmHelmet", "NameProperty", "Helmet_0_NoHelmet_M"));
            kAppearance.Add(new Property("nmEye", "NameProperty", "DefaultEyes"));
            kAppearance.Add(new Property("nmTeeth", "NameProperty", "DefaultTeeth"));
            kAppearance.Add(new Property("nmFacePropLower", "NameProperty", "Prop_FaceLower_Blank"));
            kAppearance.Add(new Property("nmFacePropUpper", "NameProperty", "Prop_FaceUpper_Blank"));
            kAppearance.Add(new Property("nmPatterns", "NameProperty", "Pat_Nothing"));
            kAppearance.Add(new Property("nmVoice", "NameProperty", "MaleVoice1_English_UK"));
            kAppearance.Add(new Property("nmLanguage", "NameProperty", "None"));
            kAppearance.Add(new Property("nmTattoo_LeftArm", "NameProperty", "Tattoo_Arms_BLANK"));
            kAppearance.Add(new Property("nm_Tattoo_RightArm", "NameProperty", "Tattoo_Arms_BLANK"));
            kAppearance.Add(new Property("nmScards", "NameProperty", "None"));
            kAppearance.Add(new Property("nmTorso_Underlay", "NameProperty", "CnvUnderlay_Std_Torsos_A_M"));
            kAppearance.Add(new Property("nmArms_Underlay", "NameProperty", "CnvUnderlay_Std_Arms_A_M"));
            kAppearance.Add(new Property("nmLegs_Underlay", "NameProperty", "CnvUnderlay_Std_Legs_A_M"));
            kAppearance.Add(new Property("nmFacePaint", "NameProperty", "None"));
            kAppearance.Add(new Property("nmLeftArm", "NameProperty", "None"));
            kAppearance.Add(new Property("nmRightArm", "NameProperty", "None"));
            kAppearance.Add(new Property("nmLeftArmDeco", "NameProperty", "None"));
            kAppearance.Add(new Property("nmRightArmDeco", "NameProperty", "None"));
            kAppearance.Add(new Property("nmLeftForearm", "NameProperty", "None"));
            kAppearance.Add(new Property("nmRightForearm", "NameProperty", "None"));
            kAppearance.Add(new Property("nmThighs", "NameProperty", "None"));
            kAppearance.Add(new Property("nmShins", "NameProperty", "None"));
            kAppearance.Add(new Property("nmTorsoDeco", "NameProperty", "None"));
            kAppearance.Add(new Property("bGhostPawn", "BoolProperty", false));

            Add(new Property("kAppearance", "StructProperty", kAppearance));

            Add(new Property("Country", "NameProperty", "Country_UK"));
            Add(new Property("AllowedTypeSoldier", "BoolProperty", true));
            Add(new Property("AllowedTypeVIP", "BoolProperty", false));
            Add(new Property("AllowedTypeDarkVIP", "BoolProperty", false));
            Add(new Property("PoolTimestamp", "StrProperty", "February 5, 2016 - 00:01"));
            Add(new Property("BackgroundText", "StrProperty", "Created using the Squaddie Character Pool Parser."));
        }

        public void AmendAppearance(string propertyName, dynamic newValue)
        {
            IProperty prop = ((List<IProperty>)(Find(x => x.Name == "kAppearance").Value)).Find(x => x.Name == propertyName);

            if (prop == null)
            {
                throw new Exception(string.Format("Property Amend Error: Property with name {0} could not be found within the appearance", propertyName));
            }

            prop.Value = newValue;
        }

        public void Amend(string propertyName, dynamic newValue)
        {
            IProperty prop = Find(x => x.Name == propertyName);

            if (prop == null)
            {
                throw new Exception(string.Format("Property Amend Error: Property with name {0} could not be found", propertyName));
            }

            prop.Value = newValue;
        }

        public new void Add(IProperty item)
        {
            //If the property already exists with the same name and type, then we should just change the value
            IProperty prop = this.Find(x => x.Name == item.Name && x.Type == item.Type);
            if (prop != null)
            {
                prop.Value = item.Value;
            }
            else
            {
                base.Add(item);
            }
        }

        public void Add(string name, string type, dynamic value)
        {
            Add(new Property(name, type, value));
        }

        public Character(Character person)
        {
            foreach (Property prop in person)
            {
                Add(new Property(prop));
            }
        }
    }
}
