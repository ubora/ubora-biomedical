Feature: WP1 Functionality tests
    As a project leader / system administrator
    I want to modify WP1/WP2 different workpackages and go through the review process

Background:
    Given I am signed in as user and on first page
        And I click on the element "p=Test title"
        And I click on the element "a=Work packages"
        And I click on the element "span=Medical need and product specification"

Scenario: I make changes in Project overview and check them
    Then I expect the title of the page "Design planning - UBORA"
    When I click on the element "span=Design planning"
        And I select value "Point-of-care diagnosis" from element "#ClinicalNeedTags"
        And I select value "Clinical microbiology" from element "#AreaOfUsageTags"
        And I select value "Mobile-based technology" from element "#PotentialTechnologyTags"
        And I set value "Magnificent other!" to the element "#Gmdn"
        And I click on the element "button=Save changes"
    Then I expect the title of the page "Design planning - UBORA"
    When I click on the element "a=Project overview"
    Then I expect the element "dd=Point-of-care diagnosis" is visible
        And I expect the element "dd=Clinical microbiology" is visible
        And I expect the element "dd=Mobile-based technology" is visible
        And I expect the element "dd=Magnificent other!" is visible
        And I expect the title of the page "Dashboard - UBORA"

Scenario: I click different Workpackages and try to edit them
    When I click on the element "span=Design planning"
    Then I expect the element "h1=Design planning" is visible
    When I click on the element "span=Medical need and product specification"
        And I click on the element "span=Clinical needs"
    Then I expect the element "h1=Clinical needs" is visible
    When I click on the element "span=Edit"
        And I click on the element "span=Helpful tips"
    Then I expect the element "p=You should describe the clinical need that is the target of your device." is visible
        And I expect the title of the page "Clinical needs - UBORA"
    When I click on the element "span=Existing solutions"
    Then I expect the element "h1=Existing solutions" is visible
    When I click on the element "span=Edit"
        And I click on the element "span=Helpful tips"
    Then I expect the element "p=You should describe what are the devices or therapies on the market; if possible, list some pros and cons of the existing solutions." is visible
        And I expect the title of the page "Existing solutions - UBORA"
    When I click on the element "span=Intended users"
    Then I expect the element "h1=Intended users" is visible
    When I click on the element "span=Edit"
        And I click on the element "span=Helpful tips"
    Then I expect the element "p=You should describe who are the intended user of your medical device (e.g. physician, technicians, nurse, midwife, family member, self-use), and where the technology will be used (rural or urban settings, at home, hospital, …)." is visible
        And I expect the title of the page "Intended users - UBORA"
    When I click on the element "span=Product requirements"
    Then I expect the element "h1=Product requirements" is visible
    When I click on the element "span=Edit"
        And I click on the element "span=Helpful tips"
    Then I expect the element "p=You should describe all the requirements to certain product. It is written to allow people to understand what a product should do. Typical components of a product requirements document are:" is visible
        And I expect the title of the page "Product requirements - UBORA"
    When I click on the element "span=Device classification"
    Then I expect the element "h1=Please answer to the following questionnaire to identify your device classification:" is visible
        And I expect the title of the page "Device classification - UBORA"
    When I click on the element "span=Regulation checklist"
    Then I expect the element "h1=Please take the questionnaire to identify applicable regulations." is visible
        And I expect the title of the page "Applicable regulations questionnaire - UBORA"
    When I click on the element "span=Formal review"
    Then I expect the element "h1=You can submit your project for review." is visible
        And I expect the title of the page "Formal review - UBORA"

Scenario: I Submit project for WP1 review
    When I click on the element "span=Formal review"
        And I click on the element "button=Submit project for review"
    Then I expect the title of the page "Formal review - UBORA"
        And I expect the element "b=Status:" is visible
        And I expect the element "td=InProcess" is visible

Scenario: System administrator adds Mentor to the project
    When I sign out
    Then I expect the title of the page "Welcome - UBORA"
    When I sign in as administrator
        And I click on the element "p=Test title"
        And I click on the element "a=Members"
    Then I expect the title of the page "Members - UBORA"
    When I click on the element "span=Invite member"
    Then I expect the title of the page "Mentors - UBORA"
    When I click on the element "button=Invite mentor"
    Then I expect the element "p=Mentor successfully invited." is visible
    When I sign out
    Then I expect the title of the page "Welcome - UBORA"

Scenario: Mentor accepts the mentor invitation
    When I sign out
        And I sign in as mentor
        And I click on the element "span=Notifications"
        And I click on the element "button=Accept"
    Then I expect the title of the page "Notifications - UBORA"
    When I click on the element "span=Projects"
    Then I expect the element "p=Test title" is visible
        And I expect the title of the page "View projects - UBORA"
    When I click on the element "p=Test title"
    Then I expect the element "a=Repository" is visible
    When I click on the element "a=Members"
    Then I expect the element "a=Test Mentor" is visible
        And I expect the element "i=school" is visible

Scenario: Project mentor rejects WP1 formal review
    When I sign out
        And I sign in as mentor
        And I click on the element "p=Test title"
        And I click on the element "a=Work packages"
        And I click on the element "span=Medical need and product specification"
        And I click on the element "span=Formal review"
        And I click on the element "a=Write a review"
    Then I expect the title of the page "Write a review - UBORA"
    When I set value "Ok project man!" to the element "#ConcludingComment"
        And I click on the element "button=Reject"
    Then I expect the element "td=Rejected" is visible
    When I sign out

Scenario: I submit my rejected WP1 again for formal review
    When I click on the element "span=Formal review"
    Then I expect the title of the page "Formal review - UBORA"
    When I click on the element "button=Submit project for review"
    Then I expect the element "td=Rejected" is visible
        And I expect the element "td=InProcess" is visible

Scenario: Project mentor accepts WP1 formal review
    When I sign out
        And I sign in as mentor
        And I click on the element "p=Test title"
        And I click on the element "a=Work packages"
        And I click on the element "span=Medical need and product specification"
        And I click on the element "span=Formal review"
        And I click on the element "a=Write a review"
        And I set value "Good project man!" to the element "#ConcludingComment"
        And I click on the element "button=Accept"
    Then I expect the element "td=Accepted" is visible
        And I expect the element "td=Good project man!" is visible
    When I sign out

Scenario: I click on WP2 work packages and try to edit them
    When I click on the element "span=Design planning"
        And I click on the element "span=Conceptual design"
        And I click on the element "span=Physical principles"
    Then I expect the element "h1=Physical principles" is visible
    When I click on the element "span=Edit"
        And I click on the element "span=Helpful tips"
    Then I expect the element "p=You should describe the most important operating physical principle of your medical device. Make sure to list every physical principal that you chose to comply to “Product requirements”." is visible
        And I expect the title of the page "Physical principles - UBORA"
    When I click on the element "span=Voting"
    Then I expect the element "h1=Voting" is visible
        And I expect the title of the page "Voting - UBORA"
    When I click on the element "span=Concept description"
    Then I expect the element "h1=Concept description" is visible
    When I click on the element "span=Edit"
        And I click on the element "span=Helpful tips"
    Then I expect the element "p=You should describe the conceptual design selected in the Voting step." is visible
        And I expect the title of the page "Concept description - UBORA"
    When I click on the element "span=Structured information on the device"
    Then I expect the title of the page "Structured information on the device - UBORA"