Feature: Home page
    As a user 
    I want to click all buttons on Home page

Background:
    Given I go to Home page

Scenario: Go to UBORA home page 
    Then I expect the title of the page "UBORA"

Scenario: Click Home button
    When I click on the element "#UboraLogo"
    Then I expect the title of the page "UBORA"

Scenario: Click Log in button
    When I click on the element "span=Log in"
    Then I expect the title of the page "Log in to UBORA - UBORA"

Scenario: Click on Community
    When I click on the element "span=Community"
    Then I expect the title of the page "Community - UBORA"

Scenario: Click on Search
    When I click on the element "span=Projects"
    Then I expect the title of the page "Biomedical device projects - UBORA"

Scenario: Check on console output
    Then I expect the console output to clear