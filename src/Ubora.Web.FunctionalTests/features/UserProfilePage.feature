Feature: User Profile page
    As a user
    I want to register an account and create my profile

Background:
    Given I go to Home page

Scenario: I sign up an account and create my profile
    When I click on the element "span=Log in"
        And I sign up as "user@profile.com"
        And I set value "Bio Bio Bio, Test Test Test" to the element "#Biography"
        And I select value "AGO" from element "#CountryCode"
        And I set value "DegreeTest" to the element "#Degree"
        And I set value "FieldTest" to the element "#Field"
        And I set value "UniversityTest" to the element "#University"
        And I select value "Researcher" from element "#MedicalDevice"
        And I set value "InstitutionTest" to the element "#Institution"
        And I set value "SkillsTest" to the element "#Skills"
        And I select value "Developer" from element "#Role"
        And I click on the element "button=Continue"
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I check my created profile
    When I click on the element "span=Profile"
        And I click on the element "a=Edit profile"
    Then I expect the title of the page "Edit profile - UBORA"
        And I expect the element "value=firstName" is visible
        And I expect the element "value=lastName" is visible
        And I expect the element "value=test@agileworks.eu" is visible
        And I expect the element "textarea=Bio Bio Bio, Test Test Test" is visible
        And I expect the element "option=Angola" is visible
        And I expect the element "value=DegreeTest" is visible
        And I expect the element "value=FieldTest" is visible
        And I expect the element "value=UniversityTest" is visible
        And I expect the element "option=Researcher" is visible
        And I expect the element "value=InstitutionTest" is visible
        And I expect the element "value=SkillsTest" is visible
        And I expect the element "option=Developer" is visible
    When I click on the element "button=Edit profile"
    Then I expect the title of the page "Manage your account - UBORA"

Scenario: I change my profile and check if my changes have been saved
    When I click on the element "span=Profile"
        And I click on the element "a=Edit profile"
        And I set value "NameFirst" to the element "#FirstName"
        And I set value "NameLast" to the element "#LastName"
        And I set value "Test Test Test, Bio Bio Bio" to the element "#Biography"
        And I select value "BGR" from element "#CountryCode"
        And I set value "TestDegree" to the element "#Degree"
        And I set value "TestField" to the element "#Field"
        And I set value "TestUniversity" to the element "#University"
        And I select value "Healthcare provider" from element "#MedicalDevice"
        And I set value "TestInstitution" to the element "#Institution"
        And I set value "TestSkills" to the element "#Skills"
        And I select value "Mentor" from element "#Role"
        And I click on the element "button=Edit profile"
        And I click on the element "a=Edit profile"
    Then I expect the title of the page "Edit profile - UBORA"
        And I expect the element "value=NameFirst" is visible
        And I expect the element "value=NameLast" is visible
        And I expect the element "value=test@agileworks.eu" is visible
        And I expect the element "textarea=Test Test Test, Bio Bio Bio" is visible
        And I expect the element "option=Bulgaria" is visible
        And I expect the element "value=TestDegree" is visible
        And I expect the element "value=TestField" is visible
        And I expect the element "value=TestUniversity" is visible
        And I expect the element "option=Healthcare provider" is visible
        And I expect the element "value=TestInstitution" is visible
        And I expect the element "value=TestSkills" is visible
        And I expect the element "option=Mentor" is visible

Scenario: I try to add an empty profile picture
    When I click on the element "span=Profile"
        And I click on the element "a=Edit profile"
        And I click on the element "button=Upload image"
    Then I expect the title of the page "Edit profile - UBORA"
        And I expect the element "span=Please select an image to upload first!" is visible

Scenario: I check terms of service
    When I click on the element "span=Profile"
        And I click on the element "a=Terms of Service"
    Then I expect the title of the page "Terms of Service - UBORA"
        And I expect the element "h1=UBORA e-infrastructure Terms of Service and Privacy Policy" is visible
        And I expect the element "a=http://e-infrastructure.ubora-biomedical.org" is visible

Scenario: I sign out
    When I sign out
    Then I expect the title of the page "Welcome - UBORA"