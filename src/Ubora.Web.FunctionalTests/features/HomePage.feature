Feature: Home page
    As a developer
    I want to click Home page button

Background:
    Given I go to the website "/Home/Index"

Scenario: Go to UBORA home page 
    Then I expect the title of the page "Welcome - UBORA"

Scenario: Click Home button
    When I click on the element ".header-logo"
    Then I expect the title of the page "Welcome - UBORA"

Scenario: Click Sign in/sign up button
    When I click on the element "a=Sign in/sign up"
    Then I expect the title of the page "Sign in - UBORA"

Scenario: Click I have an idea button
    When I click on the element ".button.primary-button"
    Then I expect the title of the page "Sign in to UBORA - UBORA"

Scenario: Click on View members
    When I click on the element ".button.secondary-button"
    Then I expect the title of the page "- UBORA"
