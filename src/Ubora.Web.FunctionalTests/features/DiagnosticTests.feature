Feature: Diagnostic tests
    As an administrator
    Diagnostic tests under TestsController should all pass when I look at them.

Background:
    Given I am signed in as administrator on first page

Scenario: All diagnostic tests pass
    When I click on the element "span=Admin"
        And I click on the element "a=Run diagnostics tests"
        And I wait for the element "span=All tests have run." for "25000" ms
    Then I expect the element "[class=diagnostics-error]" is not visible