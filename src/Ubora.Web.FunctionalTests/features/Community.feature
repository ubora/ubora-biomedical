Feature: Community page
    As a user
    I want to click on all buttons on Members page

Background:
    Given I go to Home page
        And I clicked on the element "span=Community"

Scenario: I am on View members page 
    Then I expect the title of the page "Community - UBORA"

Scenario: I click Ubora logo
    When I click on the element "#UboraLogo"
    Then I expect the title of the page "UBORA"

Scenario: I click Log in
    When I click on the element "span=Log in"
    Then I expect the title of the page "Log in to UBORA - UBORA"

Scenario: I sign in and check my profile on Community page
    When I sign in as user
        And I click on the element "span=Community"
        And I click on the element "a=Test User"
    Then I expect the title of the page "View profile - UBORA"
        And I expect the element "a=test@agileworks.eu" is visible
        And I expect the element "h2=Test User" is visible