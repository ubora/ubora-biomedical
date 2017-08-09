Feature: User list page
    As a user
    I want to click on all buttons on Members page

Background:
    Given I go to Home page
    And I clicked on the element "a=View members"

Scenario: I am on View members page 
    Then I expect the title of the page "View members - UBORA"

Scenario: I click a Ubora logo
    When I click on the element ".header-logo"
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I click a Sign in/sign up button
    When I click on the element "a=Sign in/sign up"
    Then I expect the title of the page "Sign in to UBORA - UBORA"

Scenario: I sign up and check my profile on Members page
    When I click on the element "a=Sign in/sign up"
    And I sign up as "emailemail@email.com" first name "TestName" last name "TestLastName"
    And I click on the element ".header-logo"
    And I click on the element "a=View members"
    And I click on the element "a=TestName TestLastName"
    Then I expect the title of the page "View profile - UBORA"
    And I expect the element "a=emailemail@email.com" is visible
    And I expect the element "h2=TestName TestLastName" is visible

Scenario: As a signed in user I click on my email on Members page
    When I click on the element "a=emailemail@email.com"
    Then I expect the title of the page "View members - UBORA"