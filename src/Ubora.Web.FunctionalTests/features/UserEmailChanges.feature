Feature: User email changes
    As a user
    I want to change email address on my registered account

Background:
    Given I sign up as "email-change-test@agileworks.eu" and on first page

Scenario: I change my email address
    When I click on the element "span=Profile"
        And I click on the element "a=Change email"
        And I set value "email@change.eu" to the element "#NewEmail"
        And I set value "Test12345" to the element "#Password"
        And I click on the element "button=Change email address"
    Then I expect the title of the page "Email change confirmation - UBORA"