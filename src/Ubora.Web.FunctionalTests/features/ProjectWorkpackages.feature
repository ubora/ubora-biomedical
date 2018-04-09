Feature: WP Functionality tests
    As a project leader / system administrator
    I want to modify WP1/WP2 different workpackages and go through the review process

Background:
    Given I am signed in as user and on first page
        And I click on the element "h5=Test title"
        And I click on the element "a=Work packages"
        And I click on the element "*=Medical need and product specification"

Scenario: I make changes in Project overview and check them
    Then I expect the title of the page "Design planning - UBORA"
    When I click on the element "*=Design planning"
        And I select value "Point-of-care diagnosis" from element "#ClinicalNeedTags"
        And I select value "Clinical microbiology" from element "#AreaOfUsageTags"
        And I select value "Mobile-based technology" from element "#PotentialTechnologyTags"
        And I set value "Magnificent other!" to the element "#Gmdn"
        And I click on the element "button=Save changes"
    Then I expect the title of the page "Design planning - UBORA"
    When I click on the element "a=Overview"
    Then I expect the element "dd=Point-of-care diagnosis" is visible
        And I expect the element "dd=Clinical microbiology" is visible
        And I expect the element "dd=Mobile-based technology" is visible
        And I expect the element "dd=Magnificent other!" is visible
        And I expect the title of the page "Overview - UBORA"

Scenario: I click different Workpackages and try to edit them
    When I click on the element "*=Design planning"
    Then I expect the element "h1=Design planning" is visible
    When I click on the element "*=Medical need and product specification"
        And I click on the element "*=Clinical needs"
    Then I expect the element "h1=Clinical needs" is visible
    When I click on the element "*=Edit"
        And I click on the element "*=Helpful tips"
    Then I expect the element "p=You should describe the clinical need that is the target of your device." is visible
        And I expect the title of the page "Clinical needs - UBORA"
    When I click on the element "*=Existing solutions"
    Then I expect the element "h1=Existing solutions" is visible
    When I click on the element "*=Edit"
        And I click on the element "*=Helpful tips"
    Then I expect the element "p=You should describe what are the devices or therapies on the market; if possible, list some pros and cons of the existing solutions." is visible
        And I expect the title of the page "Existing solutions - UBORA"
    When I click on the element "*=Intended users"
    Then I expect the element "h1=Intended users" is visible
    When I click on the element "*=Edit"
        And I click on the element "*=Helpful tips"
    Then I expect the element "p=You should describe who are the intended user of your medical device (e.g. physician, technicians, nurse, midwife, family member, self-use), and where the technology will be used (rural or urban settings, at home, hospital, …)." is visible
        And I expect the title of the page "Intended users - UBORA"
    When I click on the element "*=Product requirements"
    Then I expect the element "h1=Product requirements" is visible
    When I click on the element "*=Edit"
        And I click on the element "*=Helpful tips"
    Then I expect the element "p=You should describe all the requirements to certain product. It is written to allow people to understand what a product should do. Typical components of a product requirements document are:" is visible
        And I expect the title of the page "Product requirements - UBORA"
    When I click on the element "*=Device classification"
    Then I expect the element "h1=Device classification" is visible
        And I expect the element "p=Aim of this questionnaire is do offer a simple means of determining the class of a medical device according to Proposal for a Regulation of the European Parliament and of the Council on medical devices." is visible
        And I expect the title of the page "Device classification - UBORA"
    When I click on the element "*=Regulation checklist"
    Then I expect the element "h1=Regulation checklist" is visible
        And I expect the element "p=Please take the questionnaire to identify applicable regulations:" is visible
        And I expect the title of the page "Applicable regulations questionnaire - UBORA"
    When I click on the element "*=Formal review"
    Then I expect the element "h1=Formal review" is visible
        And I expect the title of the page "Formal review - UBORA"

Scenario: I Submit project for WP1 review but cancel it
    When I click on the element "*=Formal review"
        And I click on the element "button=Submit project for review"
    Then I expect the element "h5=Submit project for review" is visible
        And I expect the element "p=Work package 1 can not be edited while it is under review or after it has passed the review." is visible
    When I click on the element "button=Cancel"
    Then I expect the title of the page "Formal review - UBORA"
        And I expect the element "p=You can submit your project for review:" is visible

Scenario: I Submit project for WP1 review
    When I click on the element "*=Formal review"
        And I click on the element "button=Submit project for review"
        And I click on the element "button=Submit"
    Then I expect the element "p=WP1: Medical need and product specification can not be edited if:" is visible
        And I expect the element "li=The project is under review;" is visible
        And I expect the element "li=It passed the review;" is visible
        And I expect the element "dt=Status:" is visible
        And I expect the element "dd=InProcess" is visible

Scenario: System administrator adds Mentor to the project
    When I sign out
    Then I expect the title of the page "UBORA"
    When I sign in as administrator
        And I click on the element "h5=Test title"
        And I click on the element "a=Members"
    Then I expect the title of the page "Members - UBORA"
    When I click on the element "*=Add mentor"
    Then I expect the title of the page "Mentors - UBORA"
    When I click on the element "button=Invite mentor"
    Then I expect the element "p=Invitation sent" is visible
    When I sign out
    Then I expect the title of the page "UBORA"

Scenario: Mentor accepts the mentor invitation
    When I sign out
        And I sign in as mentor
        And I click on the element "*=Notifications"
        And I click on the element "button=Accept"
    Then I expect the title of the page "Notifications - UBORA"
    When I click on the element "*=My projects"
    Then I expect the element "h5=Test title" is visible
        And I expect the title of the page "View projects - UBORA"
    When I click on the element "h5=Test title"
    Then I expect the element "a=Repository" is visible
    When I click on the element "a=Members"
    Then I expect the element "a=Test Mentor" is visible
        And I expect the element "i=school" is visible

Scenario: Project mentor rejects WP1 formal review
    When I sign out
        And I sign in as mentor
        And I click on the element "h5=Test title"
        And I click on the element "a=Work packages"
        And I click on the element "*=Medical need and product specification"
        And I click on the element "*=Formal review"
        And I click on the element "a=Write a review"
    Then I expect the title of the page "Write a review - UBORA"
    When I set value "Ok project man!" to the element "#ConcludingComment"
        And I click on the element "button=Reject"
    Then I expect the element "dd=Rejected" is visible
    When I sign out

Scenario: I submit my rejected WP1 again for formal review
    When I click on the element "*=Formal review"
        And I click on the element "button=Submit project for review"
        And I click on the element "button=Submit"
    Then I expect the element "dd=Rejected" is visible
        And I expect the element "dt=InProcess" is visible

Scenario: Project mentor accepts WP1 formal review
    When I sign out
        And I sign in as mentor
        And I click on the element "h5=Test title"
        And I click on the element "a=Work packages"
        And I click on the element "*=Medical need and product specification"
        And I click on the element "*=Formal review"
        And I click on the element "a=Write a review"
        And I set value "Good project man!" to the element "#ConcludingComment"
        And I click on the element "button=Accept"
    Then I expect the element "dd=Accepted" is visible
        And I expect the element "dt=Good project man!" is visible
    When I sign out

Scenario: I click on WP2 work packages and try to edit them
    When I click on the element "*=Design planning"
        And I click on the element "*=Conceptual design"
        And I click on the element "*=Physical principles"
    Then I expect the element "h1=Physical principles" is visible
    When I click on the element "*=Edit"
        And I click on the element "*=Helpful tips"
    Then I expect the element "p=You should describe the most important operating physical principle of your medical device. Make sure to list every physical principal that you chose to comply to “Product requirements”." is visible
        And I expect the title of the page "Physical principles - UBORA"
    When I click on the element "*=Voting"
    Then I expect the element "h1=Voting" is visible
        And I expect the title of the page "Voting - UBORA"
    When I click on the element "*=Concept description"
    Then I expect the element "h1=Concept description" is visible
    When I click on the element "*=Edit"
        And I click on the element "*=Helpful tips"
    Then I expect the element "p=You should describe the conceptual design selected in the Voting step." is visible
        And I expect the title of the page "Concept description - UBORA"
    When I click on the element "*=Structured information on the device"
    Then I expect the title of the page "Structured information on the device - UBORA"

Scenario: I add candidate for Voting
    When I click on the element "*=Design planning"
        And I click on the element "*=Conceptual design"
        And I click on the element "*=Voting"
        And I click on the element "*=Add candidate"
        And I click on the element "button=Save changes"
    Then I expect the element "*=The Name field is required." is visible
        And I expect the element "*=The Description field is required." is visible
    When I set value "TestCandidate" to the element "#Name"
        And I set value "TestDescription" to the element "#Description"
        And I click on the element "button=Save changes"
    Then I expect the title of the page "Voting - UBORA"

Scenario: I change candidates details in Voting
    When I click on the element "*=Design planning"
        And I click on the element "*=Conceptual design"
        And I click on the element "*=Voting"
        And I click on the element "p=TestCandidate"
    Then I expect the element "h2=TestCandidate" is visible
        And I expect the element "p=TestDescription" is visible
        And I expect the title of the page "Voting - UBORA"        
    When I click on the element "*=Change image"
        And I click on the element "button=Upload image"
    Then I expect the element "*=Please select an image to upload first!" is visible
    When I click on the element "a=Discard"
        And I click on the element "button=Vote"
    Then I expect the element "*=Value for functionality must be between 1 and 5." is visible
        And I expect the element "*=Value for performance must be between 1 and 5." is visible
        And I expect the element "*=Value for usability must be between 1 and 5." is visible
        And I expect the element "*=Value for safety must be between 1 and 5." is visible
        And I expect the element ".voted=0.0" is visible
    When I click on the element "#Functionality1"
        And I click on the element "#Performace2"
        And I click on the element "#Usability3"
        And I click on the element "#Safety4"
        And I click on the element "button=Vote"
    Then I expect the element ".voted=10.0" is visible

Scenario: I add/edit a comment in Voting
    When I click on the element "*=Design planning"
        And I click on the element "*=Conceptual design"
        And I click on the element "*=Voting"
        And I click on the element "p=TestCandidate"
        And I click on the element "button=Add comment"
    Then I expect the element "*=The CommentText field is required." is visible
    When I set value "This is an awesome candidate!" to the element "#CommentText"
        And I click on the element "button=Add comment"
    Then I expect the element "span=This is an awesome candidate!" is visible
    When I click on the element "a=Test User"
    Then I expect the element "h2=Test User" is visible
    When I go back to last page
        And I click on the element "button=edit"
        And I click on the element "a=Cancel"
        And I click on the element "button=edit"
        And I set value "Still is an awesome candidate!" to the element "#CommentText"
        And I click on the element "button=Save"
    Then I expect the element "span=Still is an awesome candidate!" is visible
        And I expect the element "span=(edited)" is visible
    When I click on the element "button=delete"
    Then I expect the element "h5=Remove comment?" is visible
        And I expect the element "p=Still is an awesome candidate!" is visible
    When I click on the element "button=No, cancel"
    Then I expect the element "span=Still is an awesome candidate!" is visible
    
Scenario: I remove a comment in Voting
    When I click on the element "span=Design planning"
        And I click on the element "span=Conceptual design"
        And I click on the element "span=Voting"
        And I click on the element "p=TestCandidate"
    Then I expect the element "span=Still is an awesome candidate!" is visible
    When I click on the element "button=delete"
        And I click on the element "button=Yes, remove"
    Then I expect the element "span=Still is an awesome candidate!" is not visible

Scenario: I edit candidate's details in Voting
    When I click on the element "*=Design planning"
        And I click on the element "*=Conceptual design"
        And I click on the element "span=Voting"
        And I click on the element "p=TestCandidate"
        And I click on the element "span=Edit"
    Then I expect the element "h2=Candidate description" is visible
        And I expect the title of the page "Voting - UBORA"
    When I click on the element "a=Discard"
    Then I expect the element "h2=TestCandidate" is visible
        And I expect the element "p=TestDescription" is visible
    When I click on the element "span=Edit"
        And I set value "Candidate123" to the element "#Title"
        And I set value "Description123" to the element "#Description"
        And I click on the element "button=Save changes"
    Then I expect the element "h2=Candidate123" is visible
        And I expect the element "p=Description123" is visible
    When I click on the element "span=Voting"
    Then I expect the element "p=Candidate123" is visible
        And I expect the element "p=Description123" is visible

Scenario: I edit User and environment form
    When I click on the element "span=Design planning"
        And I click on the element "span=Conceptual design"
        And I click on the element "span=Structured information on the device"
    Then I expect the title of the page "Structured information on the device - UBORA"
    When I click on the element "#EditUserEnvironment"
    Then I expect the title of the page "User and environment - UBORA"
    When I click on the element "span=Other:"
        And I set value "Master User" to the element "#IntendedUserIfOther"
        And I click on the element "#IsTrainingRequiredInAdditionToExpectedSkillLevelOfIntentedUser"
        And I set value "Training is required!" to the element "#IfTrainingIsRequiredPleaseDescribeWhoWillDeliverTrainingAndMaterialsAndTimeRequiredForTraining"
        And I click on the element "#IsAnyMaintenanceOrCalibrationRequiredByUserAtTimeOfUse"
        And I click on the element "label=Rural settings"
        And I click on the element "label=Urban settings"
        And I click on the element "label=Outdoors"
        And I click on the element "label=Indoors"
        And I click on the element "label=At home"
        And I click on the element "label=Primary level (health post, health center)"
        And I click on the element "label=Secondary level (general hospital)"
        And I click on the element "label=Tertiary level (specialist hospital)"
        And I click on the element "button=Save"
    Then I expect the element "td=Master User" is visible
        And I expect the element "td=Training is required!" is visible
        And I expect the element "td=rural settings, urban settings, outdoors, indoors, at home, primary level (health post, health center), secondary level (general hospital), tertiary level (specialist hospital)" is visible
        And I expect the title of the page "Structured information on the device - UBORA"

Scenario: I edit Health technology specifications form
    When I click on the element "span=Design planning"
        And I click on the element "span=Conceptual design"
        And I click on the element "span=Structured information on the device"
        And I click on the element "#EditHealthTechnology"
    Then I expect the title of the page "Health technology specifications - UBORA"
    When I set value "111" to the element "#DeviceMeasurementsViewModel_DimensionsHeight"
        And I set value "222" to the element "#DeviceMeasurementsViewModel_DimensionsLength"
        And I set value "333" to the element "#DeviceMeasurementsViewModel_DimensionsWidth"
        And I set value "444" to the element "#DeviceMeasurementsViewModel_WeightInKilograms"
        And I click on the element "#DoesItRequireUseOfConsumables"
        And I set value "The list of consumables!" to the element "#IfRequiresConsumablesListConsumables"
        And I set value "365" to the element "#EstimatedLifeTimeInDays"
        And I set value "11" to the element "#EstimatedLifeTimeInMonths"
        And I set value "2017" to the element "#EstimatedLifeTimeInYears"
        And I set value "364" to the element "#EstimatedShelfLifeInDays"
        And I set value "10" to the element "#EstimatedShelfLifeInMonths"
        And I set value "2018" to the element "#EstimatedShelfLifeInYears"
        And I click on the element "#CanItHaveATelemedicineOrEHealthApplication"
        And I click on the element "#DeviceSoftwareUsageViewModel_DoesItUseAnyKindOfSoftware"
        And I set value "The software is amazing!" to the element "#DeviceSoftwareUsageViewModel_IfUsesSoftwareDescribeSoftware"
        And I set value "Yes, the software can be customized for local use!" to the element "#DeviceSoftwareUsageViewModel_IfUsesSoftwareCanSoftwareBeCustomizedForLocalUse"
        And I click on the element "label=Portable (hand-held)"
        And I click on the element "label=Reusable"
        And I click on the element "#TechnologyMaintenanceViewModel_DoesTechnologyRequireMaintenance"
        And I set value "Automatic" to the element "#TechnologyMaintenanceViewModel_IfTechnologyRequiresMaintenanceSpecifyType"
        And I set value "Static" to the element "#TechnologyMaintenanceViewModel_IfTechnologyRequiresMaintenanceSpecifyFrequency"
        And I click on the element "#TechnologyMaintenanceViewModel_IfTechnologyRequiresMaintenanceCanItBeDoneOnSiteOrHomeOrCommunity"
        And I click on the element "span=Other:"
        And I set value "Developers" to the element "#TechnologyMaintenanceViewModel_IfTechnologyRequiresMaintenanceWhoShouldProvideMaintenanceOther"
        And I click on the element "label=Mechanical energy (e.g. manually powered)"
        And I click on the element "label=Batteries"
        And I click on the element "label=Power supply for recharging (not yet developed)"
        And I set value "110" to the element "#EnergyRequirements_IfPowerSupplyForRechargingThenRequiredVoltage"
        And I set value "9999" to the element "#EnergyRequirements_IfPowerSupplyForRechargingThenRequiredTimeToRechargeInHours"
        And I set value "59" to the element "#EnergyRequirements_IfPowerSupplyForRechargingThenRequiredTimeToRechargeInMinutes"
        And I set value "10000" to the element "#EnergyRequirements_IfPowerSupplyForRechargingThenRequiredBatteryLifeInHours"
        And I set value "58" to the element "#EnergyRequirements_IfPowerSupplyForRechargingThenRequiredBatteryLifeInMinutes"
        And I click on the element "label=Continuous power supply"
        And I set value "112" to the element "#EnergyRequirements_IfContinuousPowerSupplyThenRequiredVoltage"
        And I click on the element "label=Solar power"
        And I set value "8888" to the element "#EnergyRequirements_IfSolarPowerThenTimeInSunlightRequiredToChargeInHours"
        And I set value "49" to the element "#EnergyRequirements_IfSolarPowerThenTimeInSunlightRequiredToChargeInMinutes"
        And I set value "8889" to the element "#EnergyRequirements_IfSolarPowerThenBatteryLifeInHours"
        And I set value "48" to the element "#EnergyRequirements_IfSolarPowerThenBatteryLifeInMinutes"
        And I click on the element "#EnergyRequirements_Other"
        And I set value "This is the other text!" to the element "#EnergyRequirements_OtherText"
        And I click on the element "label=Clean water supply"
        And I click on the element "label=Specific temperature and/or humidity range"
        And I set value "The specific temperature is 100 degrees!" to the element "#FacilityRequirements_IfSpecificTemperatureAndOrHumidityRangeThenDescription"
        And I click on the element "label=Clinical waste disposal facilities"
        And I set value "There are different disposal facilities!" to the element "#FacilityRequirements_IfClinicalWasteDisposalFacilitiesThenDescription"
        And I click on the element "label=Radiation isolation"
        And I click on the element "label=Gas supply"
        And I set value "Gas supply is limited!" to the element "#FacilityRequirements_IfGasSupplyThenDescription"
        And I click on the element "label=Sterilization"
        And I set value "No sterilization is actually required!" to the element "#FacilityRequirements_IfSterilizationThenDescription"
        And I click on the element "label=Access to the Internet"
        And I click on the element "label=Access to a cellular phone network"
        And I click on the element "label=Connection to a laptop/computer"
        And I click on the element "label=Accessible by car"
        And I click on the element "label=Additional sound / light control facilities"
        And I click on the element "#FacilityRequirements_Other"
        And I set value "There are some more facility requirements!" to the element "#FacilityRequirements_OtherText"
        And I click on the element "button=Save"
    Then I expect the element "td=222 mm x 333 mm x 111 mm" is visible
        And I expect the element "td=444 kg" is visible
        And I expect the element "td=Yes. The list of consumables!" is visible
        And I expect the element "td=365 Days 11 Months 2017 Years" is visible
        And I expect the element "td=364 Days 10 Months 2018 Years" is visible
        And I expect the element "td=The software is amazing!" is visible
        And I expect the element "td=Yes, the software can be customized for local use!" is visible
        And I expect the element "td=Portable" is visible
        And I expect the element "td=Reusable" is visible
        And I expect the element "p=Type: Automatic" is visible
        And I expect the element "p=Frequency: Static" is visible
        And I expect the element "p=Can it be done on-site / home / community?: Yes" is visible
        And I expect the element "td=Developers" is visible
        And I expect the element "p=Mechanical energy (e.g. manually powered)" is visible
        And I expect the element "span=Power supply for recharging" is visible
        And I expect the element "span=Voltage required: 110 V" is visible
        And I expect the element "span=Time required to recharge: 9999 hours 59 minutes" is visible
        And I expect the element "span=Battery life with full charge: 10000 hours 58 minutes" is visible
        And I expect the element "span=Continuous power supply" is visible
        And I expect the element "span=Voltage required: 112 V" is visible
        And I expect the element "span=Solar power" is visible
        And I expect the element "span=Time in sunlight required to charge: 8888 hours 49 minutes" is visible
        And I expect the element "span=Battery life with full charge: 8889 hours 48 minutes" is visible
        And I expect the element "p=This is the other text!" is visible
        And I expect the element "p=Clean water supply" is visible
        And I expect the element "p=Specific temperature/humidity range (description: The specific temperature is 100 degrees!)" is visible
        And I expect the element "p=Clinical waste disposal facilities (description: There are different disposal facilities!)" is visible
        And I expect the element "p=Gas supply (description: Gas supply is limited!)" is visible
        And I expect the element "p=Sterilization (description: No sterilization is actually required!)" is visible
        And I expect the element "p=Radiation isolation" is visible
        And I expect the element "p=Access to the Internet" is visible
        And I expect the element "p=Access to a cellular phone network" is visible
        And I expect the element "p=Connection to a laptop/computer" is visible
        And I expect the element "p=Accessible by car" is visible
        And I expect the element "p=Additional sound / light control facilities" is visible
        And I expect the element "p=There are some more facility requirements!" is visible
        And I expect the title of the page "Structured information on the device - UBORA"

Scenario: I open WP3
    When I click on the element "span=Design planning"
        And I click on the element "span=Conceptual design"
        And I click on the element "span=Voting"
        And I click on the element "span=Open “WP3: Design and prototyping”"
    Then I expect the element "h5=Open “Work package 3: design and prototyping”" is visible
        And I expect the element "p=Please be sure you have reached consensus on the conceptual design of your medical device. Are you sure you want to open “WP3: Design and prototyping”?" is visible
    When I click on the element "button=No, cancel"
        And I click on the element ".legal-flag"
        And I click on the element "span=Open “WP3: Design and prototyping”"
        And I click on the element "button=Yes, open"
    Then I expect the element "p=Work package 3 opened successfully!" is visible

Scenario: I click on General product description work packages and try to edit them
    When I click on the element "span=Design planning"
        And I click on the element "span=Design and prototyping"
        And I click on the element "span=General product description"
        And I click on the element "span=Hardware"
        And I click on the element "span=Commercial parts"
    Then I expect the title of the page "Commercial parts - UBORA"
        And I expect the element "h1=Commercial parts" is visible
    When I click on the element "span=Edit"
        And I click on the element "span=Helpful tips"
    Then I expect the element "p=Please insert here design and description of mechanical parts of your device which you can buy from suppliers (e.g. box, valves, …)" is visible
        And I expect the title of the page "Commercial parts - UBORA"
    When I click on the element "span=Purposely designed parts"
    Then I expect the title of the page "Purposely designed parts - UBORA"
        And I expect the element "h1=Purposely designed parts" is visible
    When I click on the element "span=Edit"
        And I click on the element "span=Helpful tips"
    Then I expect the element "p=Please insert here design and description of mechanical parts of your device which you created from scratch (e.g. customized gears, purposely designed frames)" is visible
        And I expect the title of the page "Purposely designed parts - UBORA"
    When I click on the element "span=Prototypes and functional trials"
    Then I expect the title of the page "Prototypes and functional trials - UBORA"
        And I expect the element "h1=Prototypes and functional trials" is visible
    When I click on the element "span=Edit"
        And I click on the element "span=Helpful tips"
    Then I expect the element "p=Please insert here how to fabricate the purposely designed parts (e.g. gcode for 3D printers, CNC fabrication cycle with nc code) and describe step-by-step how to assemble all the components." is visible
        And I expect the title of the page "Prototypes and functional trials - UBORA"
    When I click on the element "span=Electronic & firmware"
        And I click on the href element "/WP3/GeneralProductDescription_ElectronicAndFirmware_CommercialParts"
    Then I expect the title of the page "Commercial parts - UBORA"
        And I expect the element "h1=Commercial parts" is visible
    When I click on the element "span=Edit"
        And I click on the element "span=Helpful tips"
    Then I expect the element "p=Please insert here design and description of electronic parts of your device which you can buy from suppliers (e.g. microcontrollers, sensors, I/O boards, …). Describe all the libraries you use in your firmware." is visible
        And I expect the title of the page "Commercial parts - UBORA"
    When I click on the href element "/WP3/GeneralProductDescription_ElectronicAndFirmware_PurposelyDesignedParts"
    Then I expect the title of the page "Purposely designed parts - UBORA"
        And I expect the element "h1=Purposely designed parts" is visible
    When I click on the element "span=Edit"
        And I click on the element "span=Helpful tips"
    Then I expect the element "p=Please insert here design and description of electronic parts of your device which you created from scratch (e.g. circuits, filters, routing ). You should include also CAD data as schematics and gerber files." is visible
        And I expect the title of the page "Purposely designed parts - UBORA"
    When I click on the href element "/WP3/GeneralProductDescription_ElectronicAndFirmware_PrototypesAndFunctionalTrials"
    Then I expect the title of the page "Prototypes and functional trials - UBORA"
        And I expect the element "h1=Prototypes and functional trials" is visible
    When I click on the element "span=Edit"
        And I click on the element "span=Helpful tips"
    Then I expect the element "p=Please insert here how to assemble step-by step the electronic part of your device, including instruction on how to upload the firmware." is visible
        And I expect the title of the page "Prototypes and functional trials - UBORA"
    When I click on the element "span=Software"
        And I click on the element "span=Existing solutions (open source)"
    Then I expect the title of the page "Existing solutions (open source) - UBORA"
        And I expect the element "h1=Existing solutions (open source)" is visible
    When I click on the element "span=Edit"
        And I click on the element "span=Helpful tips"
    Then I expect the element "p=Please insert here free and open source libraries and programs you use in your device developed by other authors, without forgetting due credits." is visible
        And I expect the title of the page "Existing solutions (open source) - UBORA"
    When I click on the href element "/WP3/GeneralProductDescription_Software_PurposelyDesignedParts"
    Then I expect the title of the page "Purposely designed parts - UBORA"
        And I expect the element "h1=Purposely designed parts" is visible
    When I click on the element "span=Edit"
        And I click on the element "span=Helpful tips"
    Then I expect the element "p=Please describe here your code using flowcharts and maps. If your code implements specific algorithms, please include comments in each key point." is visible
        And I expect the title of the page "Purposely designed parts - UBORA"
    When I click on the href element "/WP3/GeneralProductDescription_Software_PrototypesAndFunctionalTrials"
    Then I expect the title of the page "Prototypes and functional trials - UBORA"
        And I expect the element "h1=Prototypes and functional trials" is visible
    When I click on the element "span=Edit"
        And I click on the element "span=Helpful tips"
    Then I expect the element "p=Please describe here how to install you software (including specific requirements in terms of operating system, RAM, CPU, library dependencies." is visible
        And I expect the title of the page "Prototypes and functional trials - UBORA"
    When I click on the element "span=System integration"
        And I click on the href element "/WP3/GeneralProductDescription_SystemIntegration_PrototypesAndFunctionalTrials"    
    Then I expect the title of the page "Prototypes and functional trials - UBORA"
        And I expect the element "h1=Prototypes and functional trials" is visible
    When I click on the element "span=Edit"
        And I click on the element "span=Helpful tips"
    Then I expect the element "p=Describe here how to integrate all the parts of your device, including wiring and connections." is visible
        And I expect the title of the page "Prototypes and functional trials - UBORA"
    
Scenario: I click and edit two last WP3 work packages
    When I click on the element "*=Design planning"
        And I click on the element "*=Design and prototyping"
        And I click on the element "*=Design for ISO testing compliance"
    Then I expect the title of the page "Design for ISO testing compliance - UBORA"
        And I expect the element "h1=Design for ISO testing compliance" is visible
    When I click on the element "*=Edit"
        And I click on the element "*=Helpful tips"
    Then I expect the element "p=With the help of your mentors, identify and design the necessary tests to demonstrate the compliance of your design with relevant ISO standards." is visible
        And I expect the title of the page "Design for ISO testing compliance - UBORA"
    When I click on the element "*=Instructions for fabrication of prototypes"
    Then I expect the title of the page "Instructions for fabrication of prototypes - UBORA"
        And I expect the element "h1=Instructions for fabrication of prototypes" is visible
    When I click on the element "*=Edit"
        And I click on the element "*=Helpful tips"
    Then I expect the element "p=Use this last section of WP3 to describe step by step how to fabricate and assemble the last version of your prototype: fill it once the part 1 and part 2 of WP3 can be considered stable." is visible
        And I expect the title of the page "Instructions for fabrication of prototypes - UBORA"